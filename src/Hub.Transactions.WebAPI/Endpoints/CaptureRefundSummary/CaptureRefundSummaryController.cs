using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.CaptureRefundSummary
{
    [Route("[controller]")]
    public class CaptureRefundSummaryController : Controller
    {
        private readonly IDatabase _db;

        public CaptureRefundSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<CaptureRefundSummaryRow> Get([FromQuery]CaptureRefundSummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("(txn_type = 'RefundPayment' OR txn_type = 'CapturePayment' OR txn_type = 'OneStepPayment' OR txn_type = 'CreditPayment')");
            criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id = (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "exch_currency IN (@ExchangeCurrencies)");
            criteria.AddIfNotNull(args.TransactionTypes, "txn_type IN (@TransactionTypes)");            

            var where = criteria.ToWhereClause();

            var sql = @"select 
                        COALESCE(NULLIF(ts.api_name,''), '[unknown]') AS Product, 
                        COALESCE(NULLIF(tx.bin_country,''), '[unknown]') AS BINCountry, 
                        tx.mid as MID, 
                        tx.corp_id AS CorporateId, 
                        tx.corp_name AS Corporate,
                        tx.mongo_bank_id AS BankId,
                        tx.bank_name AS Bank,
                        (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS Date,  
                        case when tx.card_type is null or tx.card_type = '' then tx.method_name else tx.card_type end as CardType, 
                        tx.txn_type as TransactionType,  
                        tx.currency as Currency,
                        Count(*) as Count,  
                        sum(amount*.001) as TotalAmount, 
                        COALESCE(NULLIF(tx.method_name,''), '[unknown]') AS PaymentMethod, 
                        avg(case when tx.exch_rate > '' then cast(tx.exch_rate as numeric) else 0 end) as ExchangeRate,
                        sum(case when tx.exch_amount > '' then cast(tx.exch_amount as numeric)*.001 else 0 end) as ExchangeAmount,
                        tx.exch_currency as ExchangeCurrency 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        group by ts.api_name, tx.mid, tx.corp_id, tx.corp_name, tx.bank_name, tx.mongo_bank_id, Date, tx.card_type, tx.bin_country, tx.txn_type, tx.currency, tx.exch_currency, tx.method_name
                        order by api_name, mid, tx.corp_id, tx.corp_name, Date, card_type, tx.bin_country, txn_type, currency, method_name, tx.exch_currency";

            var result = _db.FetchPagedResult<CaptureRefundSummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]CaptureRefundSummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("(txn_type = 'RefundPayment' OR txn_type = 'CapturePayment' OR txn_type = 'OneStepPayment' OR txn_type = 'CreditPayment')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id = (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "Currency", optionsArgs.OptionValue, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "ExchangeCurrency", optionsArgs.OptionValue, "exch_currency IN (@ExchangeCurrencies)");
            criteria.AddIfNotNull(args.TransactionTypes, "TransactionType", optionsArgs.OptionValue, "txn_type IN (@TransactionTypes)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        group by ts.api_name, tx.mid, tx.corp_id, tx.corp_name, tx.req_rcv_at_main_flw::date, tx.card_type, tx.bin_country, tx.txn_type, tx.currency, tx.exch_currency, tx.method_name";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
