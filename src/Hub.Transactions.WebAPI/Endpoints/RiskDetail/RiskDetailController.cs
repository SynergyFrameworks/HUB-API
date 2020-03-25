using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.RiskDetail
{
    [Route("[controller]")]
    public class RiskDetailController : Controller
    {
        private readonly IDatabase _db;

        public RiskDetailController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<RiskDetailRow> Get([FromQuery]RiskDetailArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("mid <> ''");
            criteria.Add("txn_type IN ('AddrValidation', 'DocumentCheck', 'EmailCheck', 'FraudScreen', 'IDCheck', 'EnrolCheck', 'Tokenize')");
            criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id = (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.ResponseCodes, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.BIN, "bin LIKE @BIN", () => args.BIN = string.Format("%{0}%", args.BIN));
            criteria.AddIfNotNull(args.PaymentMethods, "method_name = @PaymentMethods");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "exch_currency IN (@ExchangeCurrencies)");
            criteria.AddIfNotNull(args.EmailAddress, "customer_email_address LIKE @EmailAddress", () => args.EmailAddress = string.Format("%{0}%", args.EmailAddress));
            criteria.AddIfNotNull(args.MerchantTransactionID, "m_txn_id LIKE @MerchantTransactionID", () => args.MerchantTransactionID = string.Format("%{0}%", args.MerchantTransactionID));
            criteria.AddIfNotNull(args.TransactionTypes, "txn_type IN (@TransactionTypes)");
            criteria.AddIfNotNull(args.Product, "ts.api_name = @Product");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT 
                          ts.id as ID, 
                          tx.global_id AS GlobalId, 
                          tx.global_name AS Global, 
                          mongo_bank_id as BankId,
                          bank_name as Bank,
                          tx.corp_id as CorporateId, 
                          tx.corp_name as Corporate, 
                          mid as MID,
                          mongo_m_id as MerchantId,
                          merchant_name as Merchant,
                          m_txn_id as MerchantTransactionID,
                          txn_type as TransactionType,
                          ts.api_name as Product,
                          ts.api_id as ProductId,
                          tx.resp_code as ResponseCode,
                          tx.resp_code_desc as ResponseCodeDescription,
                          COALESCE(NULLIF(method_name,''), '[unknown]') AS PaymentMethod,
                          currency as Currency,
                          amount*.001 as Amount,
                          billing_address_customer_name as CustomerName,
                          customer_email_address as CustomerEmailAddress,
                          customer_ip_address as CustomerIPAddress,
                          tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval AS Date, 
                          case when tx.exch_rate > '' then cast(tx.exch_rate as numeric) else 0 end as ExchangeRate,
                          case when tx.exch_amount > '' then cast(tx.exch_amount as numeric)*.001 else 0 end as ExchangeAmount,
                          exch_currency as ExchangeCurrency,
                          fr.score as RiskScore,
                          case when tx.resp_code = '1000' then 'Pass' else 'Fail' end as RiskStatus
                          from cs_rpt_txn tx
                          LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                          LEFT JOIN fraud_result fr ON tx.cs_rpt_txn_id = fr.cs_rpt_txn_id
                          " + where + @"
                          order by req_rcv_at_main_flw DESC";

            var result = _db.FetchPagedResult<RiskDetailRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]RiskDetailArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("mid <> ''");
            criteria.Add("txn_type IN ('AddrValidation', 'DocumentCheck', 'EmailCheck', 'FraudScreen', 'IDCheck', 'EnrolCheck', 'Tokenize')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id = (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "Currency", optionsArgs.OptionValue, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.ResponseCodes, "ResponseCode", optionsArgs.OptionValue, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.BIN, "BIN", optionsArgs.OptionValue, "bin LIKE @BIN", () => args.BIN = string.Format("%{0}%", args.BIN));
            criteria.AddIfNotNull(args.PaymentMethods, "PaymentMethod", optionsArgs.OptionValue, "method_name = @PaymentMethods");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "ExchangeCurrency", optionsArgs.OptionValue, "exch_currency IN (@ExchangeCurrencies)");
            criteria.AddIfNotNull(args.EmailAddress, "customer_email_address LIKE @EmailAddress", () => args.EmailAddress = string.Format("%{0}%", args.EmailAddress));
            criteria.AddIfNotNull(args.MerchantTransactionID, "m_txn_id LIKE @MerchantTransactionID", () => args.MerchantTransactionID = string.Format("%{0}%", args.MerchantTransactionID));
            criteria.AddIfNotNull(args.TransactionTypes, "TransactionType", optionsArgs.OptionValue, "txn_type IN (@TransactionTypes)");
            criteria.AddIfNotNull(args.Product, "Product", optionsArgs.OptionValue, "ts.api_name = @Product");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
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
