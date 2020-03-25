using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;
using System.Linq;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionBySettlementDate
{
    [Route("[controller]")]
    public class TransactionBySettlementDateController
    {
        private readonly IDatabase _db;

        public TransactionBySettlementDateController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<TransactionBySettlementDateRow> Get([FromQuery]TransactionBySettlementDateArgs args)
        {
            args.TransactionTypes = args.TransactionTypes ?? new List<string> { "OneStepPayment", "RefundPayment", "CapturePayment" };
            args.ResponseCodes = (args.ResponseCodes ?? new List<string>()).Union(new[] { "1000" }).ToList();

            var criteria = new List<string>();

            criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
            criteria.AddIfNotNull(args.DateFrom, "settlement_date >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "settlement_date <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.TransactionTypes, "txn_type IN (@TransactionTypes)");
            criteria.AddIfNotNull(args.ResponseCodes, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.MerchantTransactionID, "m_txn_id LIKE @MerchantTransactionID", () => args.MerchantTransactionID = string.Format("%{0}%", args.MerchantTransactionID));
            criteria.AddIfNotNull(args.Product, "ts.api_name = @Product");

            var where = criteria.ToWhereClause();

            var sql = @"select 
                        (tx.settlement_date::timestamp without time zone + (@TimeZoneOffset ||' minutes')::interval)::date as Date,  
                        txn_type as TransactionType,
                        ts.api_name as Product,
                        tx.resp_code as ResponseCode,
                        currency as Currency,
                        amount*.001 as Amount,
                        m_txn_id as MerchantTransactionID,
                        bin as BIN,
                        acc_holder as AccountHolder,
                        crypto_key as CryptoId,
                        tx.global_id AS GlobalId, 
                        tx.global_name AS Global, 
                        tx.mongo_bank_id as BankId, 
                        tx.bank_name as Bank, 
                        tx.mongo_m_id as MerchantId, 
                        tx.mid as MID, 
                        tx.corp_id as CorporateId, 
                        tx.corp_name as Corporate,
                        requestor_origin as RequestorOrigin, 
                        req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval as TransactionDate, 
                        tx.provider_reference as ProviderReference, 
                        card_last_four_digits as CardLastFourDigits, 
                        ts.provider_resp_code as ProviderResponseCode 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                        " + where + @"
                        order by Date";

            var result = _db.FetchPagedResult<TransactionBySettlementDateRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]TransactionBySettlementDateArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            args.TransactionTypes = args.TransactionTypes ?? new List<string> { "OneStepPayment", "RefundPayment", "CapturePayment" };
            args.ResponseCodes = (args.ResponseCodes ?? new List<string>()).Union(new[] { "1000" }).ToList();

            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "settlement_date >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "settlement_date <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.TransactionTypes, "txn_type IN (@TransactionTypes)");
            criteria.AddIfNotNull(args.ResponseCodes, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.MerchantTransactionID, "m_txn_id LIKE @MerchantTransactionID", () => args.MerchantTransactionID = string.Format("%{0}%", args.MerchantTransactionID));
            criteria.AddIfNotNull(args.Product, "ts.api_name = @Product");

            var where = criteria.ToWhereClause();

            var sql = @"select distinct  
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                        " + where;

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
