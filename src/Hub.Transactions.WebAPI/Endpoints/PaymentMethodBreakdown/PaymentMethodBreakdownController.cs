using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.PaymentMethodBreakdown
{
    [Route("[controller]")]
    public class PaymentMethodBreakdownController : Controller
    {
        private readonly IDatabase _db;

        public PaymentMethodBreakdownController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<PaymentMethodBreakdownRow> Get([FromQuery]PaymentMethodBreakdownArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code IN ('1000', '1042')");
            criteria.Add("(txn_type = 'CapturePayment' OR txn_type = 'OneStepPayment' OR txn_type = 'OnlinePayment')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var with = @"WITH tmp_table AS (select mid, currency, case when bin_country is null or bin_country = '' then '[unknown]' else bin_country end, count(*) as total_count, sum(amount) as total_value from cs_rpt_txn tx
                        " + where + @" 
                        group by mid, currency, tx.bin_country order by mid, currency, bin_country) ";

            var sql = with + @"select 
                                tx.global_name AS Global, 
                                tx.global_id AS GlobalId, 
                                tx.bank_name AS Bank, 
                                tx.mongo_bank_id AS Bank_ID, 
                                tx.corp_name AS Corporate, 
                                tx.corp_id AS CorporateId, 
                                tx.merchant_name AS Merchant, 
                                tx.mongo_m_id AS MerchantID, 
                                tx.mid AS MID,
                                (case when tx.currency is null or tx.currency = '' then '[unknown]' else tx.currency end) as Currency,
                                (case when tx.method_name is null or tx.method_name = '' then tx.card_type else tx.method_name end) as PaymentMethod,
                                (case when tx.bin_country is null or tx.bin_country = '' then '[unknown]' else tx.bin_country end) as BINCountry,
                                count(*) as CaptureCount,
                                round(count(*)/tmp.total_count::decimal, 2) * 100  as CaptureCountPercentage,
                                sum(amount) as CaptureValue,
                                round(sum(amount)/tmp.total_value * 100 ::decimal, 2) as CaptureValuePercentage,
                                round(sum(amount)/count(*)::decimal, 0) as AverageValue
                                from cs_rpt_txn tx
                                LEFT JOIN tmp_table tmp on tmp.currency = tx.currency and tmp.bin_country = case when tx.bin_country is null or tx.bin_country = '' then '[unknown]' else tx.bin_country end
                                " + where + @"
                                group by tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.corp_name, tx.corp_id, tx.merchant_name, tx.mongo_m_id, tx.mid, tx.currency, tx.method_name, tx.bin_country, tmp.total_count, tmp.total_value,tx.card_type
                                order by tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.corp_name, tx.corp_id, tx.merchant_name, tx.mongo_m_id, tx.mid, tx.currency, tx.method_name, tx.bin_country, tmp.total_count, tmp.total_value,tx.card_type";

            var result = _db.FetchPagedResult<PaymentMethodBreakdownRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]PaymentMethodBreakdownArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("(txn_type = 'CapturePayment' OR txn_type = 'OneStepPayment')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var with = @"WITH tmp_table AS (select mid, currency, case when bin_country is null or bin_country = '' then '[unknown]' else bin_country end, count(*) as total_count, sum(amount) as total_value from cs_rpt_txn tx
                        " + where + @" 
                        group by mid, currency, tx.bin_country order by mid, currency, bin_country) ";

            var sql = with + @"SELECT DISTINCT 
                                " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                                " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                                from cs_rpt_txn tx
                                LEFT JOIN tmp_table tmp on tmp.currency = tx.currency and tmp.bin_country = case when tx.bin_country is null or tx.bin_country = '' then '[unknown]' else tx.bin_country end
                                " + where + @"
                                group by tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.corp_name, tx.corp_id, tx.merchant_name, tx.mongo_m_id, tx.mid, tx.currency, tx.method_name, tx.bin_country, tmp.total_count, tmp.total_value";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
