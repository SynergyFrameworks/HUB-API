using System.Collections.Generic;
using System.Linq;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.RefundRates
{
    [Route("[controller]")]
    public class RefundRatesController : Controller
    {
        private readonly IDatabase _db;

        public RefundRatesController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<RefundRatesRow> Get([FromQuery]RefundRatesArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("tx.txn_type NOT IN ('PaymentStatus','VerifyAccount','ShippingService')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "exch_currency IN (@ExchangeCurrencies)");

            var where = criteria.ToWhereClause();

            var sql = @"select 
                        ts.api_name as Product, 
                        tx.mid as MID, 
                        tx.corp_id AS CorporateId, 
                        tx.corp_name AS Corporate, 
                        tx.currency as Currency, 
                        case when tx.card_type is null or tx.card_type = '' then tx.method_name else tx.card_type end as CardType, 
                        (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS Date,  
                        sum(case when tx.txn_type = 'CapturePayment' or tx.txn_type = 'OneStepPayment' then amount*.001 else 0 end) as SettlementAmount,
                        sum(case when tx.txn_type = 'CapturePayment' or tx.txn_type = 'OneStepPayment' then 1 else 0 end) as SettlementCount,  
                        sum(case when tx.txn_type = 'RefundPayment' or tx.txn_type = 'CreditPayment' then amount*.001 else 0 end) as RefundAmount,
                        sum(case when tx.txn_type = 'RefundPayment' or tx.txn_type = 'CreditPayment' then 1 else 0 end) as RefundCount,
                        avg(case when tx.exch_rate > '' then cast(tx.exch_rate as numeric) else 0 end) as ExchangeRate,
                        sum(case when tx.exch_amount > '' then cast(tx.exch_amount as numeric)*.001 else 0 end) as ExchangeAmount,
                        tx.exch_currency as ExchangeCurrency
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        group by ts.api_name, tx.mid, tx.corp_id, tx.corp_name, Date, tx.card_type, tx.currency, tx.exch_currency,tx.method_name
                        order by api_name, mid, tx.corp_id, tx.corp_name, Date, card_type, tx.currency, tx.exch_currency,tx.method_name";

            var result = _db.FetchPagedResult<RefundRatesRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]RefundRatesArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "Currency", optionsArgs.OptionValue, "currency IN (@Currencies)");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "ExchangeCurrency", optionsArgs.OptionValue, "exch_currency IN (@ExchangeCurrencies)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        group by ts.api_name, tx.mid, tx.corp_id, tx.corp_name, tx.req_rcv_at_main_flw::date, tx.card_type, tx.currency, tx.exch_currency";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
