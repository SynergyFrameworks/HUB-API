using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.CardTypeBreakdown
{
    [Route("[controller]")]
    public class CardTypeBreakdownController : Controller
    {
        private readonly IDatabase _db;

        public CardTypeBreakdownController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<CardTypeBreakdownRow> Get([FromQuery]CardTypeBreakdownArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("(txn_type = 'CapturePayment' OR txn_type = 'OneStepPayment')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var sql = @"select COALESCE(NULLIF(ts.api_name,''), '[unknown]') AS Product, tx.corp_id AS CorporateId, tx.corp_name AS Corporate, 
                        COALESCE(NULLIF(tx.bin_country,''), '[unknown]') AS BINCountry, 
                        case when tx.card_type is null or tx.card_type = '' then '[unknown]' else tx.card_type end as CardType, 
                        sum(case when tx.txn_type = 'CapturePayment' or tx.txn_type = 'OneStepPayment' then 1 else 0 end) as CapturePaymentCount  
                        from cs_rpt_txn tx LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        group by api_name, tx.corp_id, tx.corp_name, card_type, tx.bin_country
                        order by api_name, tx.corp_id, tx.corp_name, card_type, tx.bin_country";

            var result = _db.FetchPagedResult<CardTypeBreakdownRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]CardTypeBreakdownArgs args, [FromQuery]OptionArgs optionsArgs)
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

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        group by api_name, tx.corp_id, tx.corp_name, card_type, tx.bin_country";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
