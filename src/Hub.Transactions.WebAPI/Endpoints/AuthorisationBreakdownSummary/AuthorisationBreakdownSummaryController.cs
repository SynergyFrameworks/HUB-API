using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.AuthorisationBreakdownSummary
{
    [Route("[controller]")]
    public class AuthorisationBreakdownSummaryController : Controller
    {
        private readonly IDatabase _db;

        public AuthorisationBreakdownSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<AuthorisationBreakdownSummaryRow> Get([FromQuery]AuthorisationBreakdownSummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var sql = $@"SELECT 
                        ts.api_name AS Product, 
                        mid AS MID, 
                        tx.corp_id AS CorporateId, 
                        tx.corp_name AS Corporate, 
                        case when card_type is null or card_type = '' then tx.method_name else card_type end AS CardType, 
                        ts.resp_code as ResponseCode,  
                        ts.resp_code_desc as ResponseCodeDescription, 
                        count(*) as Count, 
                        (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS TransactionDate 
                        FROM cs_rpt_txn tx 
				        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                {where}
		                AND tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','VerifyAccount')
		                GROUP BY api_name, mid, tx.corp_id, tx.corp_name, card_type, TransactionDate, ts.resp_code, ts.resp_code_desc,tx.method_name 
		                ORDER BY api_name, mid, tx.corp_id, tx.corp_name, card_type, TransactionDate, ts.resp_code, ts.resp_code_desc,tx.method_name";

            var transactions = _db.FetchPagedResult<AuthorisationBreakdownSummaryRow>(args.Page, args.PageSize, sql, args);

            return transactions;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]AuthorisationBreakdownSummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

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
                        FROM cs_rpt_txn tx 
				        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                " + where + @"
		                AND tx.txn_type IN (AuthPayment','IncrementalAuthPayment','OneStepPayment','VerifyAccount')
		                GROUP BY api_name, mid, tx.corp_id, tx.corp_name, card_type, req_rcv_at_main_flw::date, ts.resp_code, ts.resp_code_desc";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
