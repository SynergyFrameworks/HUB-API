using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.RiskSummary
{
    [Route("[controller]")]
    public class RiskSummaryController : Controller
    {
        private readonly IDatabase _db;

        public RiskSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<RiskSummaryRow> Get([FromQuery]RiskSummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("txn_type IN ('AddrValidation', 'DocumentCheck',  'EmailCheck', 'FraudScreen', 'IDCheck', 'EnrolCheck', 'Tokenize')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            
            var where = criteria.ToWhereClause();

            var with = @"WITH tmp_table AS (select mid, (req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date as date, ts.api_name, count(*) as total_count from cs_rpt_txn tx 
                        LEFT JOIN cs_rpt_timestamp ts on ts.cs_rpt_txn_id = tx.cs_rpt_txn_id 
                        " + where + @" 
                        group by mid, date, ts.api_name order by mid, date, ts.api_name) ";

            var sql = with + @"select 
                                tx.global_name AS Global, 
                                tx.global_id AS GlobalId, 
                                tx.bank_name as Bank, 
                                tx.mongo_bank_id as BankId, 
                                tx.merchant_name as Merchant, 
                                tx.mongo_m_id as MerchantId, 
                                tx.mid as MID, 
                                tx.corp_id as CorporateId, 
                                tx.corp_name as Corporate, 
                                (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS TransactionDate,
                                ts.api_name as Product,
                                tx.resp_code as ResponseCode, 
                                tx.resp_code_desc as ResponseDescription,
                                tx.txn_type as TransactionType,
                                count(*) as Count,
                                round(count(*)/tmp.total_count::decimal * 100, 2) as StatusPercentage,
                                case when tx.resp_code = '1000' then 'Pass' else 'Fail' end as Status
                                from cs_rpt_txn tx 
                                JOIN cs_rpt_timestamp ts on ts.cs_rpt_txn_id = tx.cs_rpt_txn_id
                                LEFT JOIN tmp_table tmp on tmp.mid = tx.mid and tmp.date = (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date and tmp.api_name =  ts.api_name
                                " + where + @"
                                group by tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.merchant_name, tx.mongo_m_id, tx.mid, tx.corp_id, tx.corp_name, TransactionDate, ts.api_name, tx.resp_code, tx.resp_code_desc, tx.txn_type, tmp.total_count
                                order by tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.merchant_name, tx.mongo_m_id, tx.mid, tx.corp_id, tx.corp_name, TransactionDate, ts.api_name, tx.resp_code, tx.resp_code_desc, tx.txn_type, tmp.total_count";

            var result = _db.FetchPagedResult<RiskSummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]RiskSummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("txn_type IN ('AddrValidation', 'DocumentCheck',  'EmailCheck', 'FraudScreen', 'IDCheck', 'EnrolCheck', 'Tokenize')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id IN (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var with = @"WITH tmp_table AS (select mid, req_rcv_at_main_flw::date as date, ts.api_name, count(*) as total_count from cs_rpt_txn tx 
                        LEFT JOIN cs_rpt_timestamp ts on ts.cs_rpt_txn_id = tx.cs_rpt_txn_id 
                        " + where + @" 
                        group by mid, req_rcv_at_main_flw::date, ts.api_name order by mid, req_rcv_at_main_flw::date, ts.api_name) ";

            var sql = with + @"SELECT DISTINCT 
                                " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                                " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                                from cs_rpt_txn tx 
                                JOIN cs_rpt_timestamp ts on ts.cs_rpt_txn_id = tx.cs_rpt_txn_id
                                LEFT JOIN tmp_table tmp on tmp.mid =  tx.mid and tmp.date =  tx.req_rcv_at_main_flw::date and tmp.api_name =  ts.api_name
                                " + where + @"
                                group by tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.merchant_name, tx.mongo_m_id, tx.mid, tx.corp_id, tx.corp_name, tx.req_rcv_at_main_flw::date, ts.api_name, tx.resp_code, tx.resp_code_desc, tx.txn_type, tmp.total_count";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
