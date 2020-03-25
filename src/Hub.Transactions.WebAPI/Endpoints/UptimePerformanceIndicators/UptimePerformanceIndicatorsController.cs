using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.UptimePerformanceIndicators
{
    [Route("[controller]")]
    public class UptimePerformanceIndicatorsController : Controller
    {
        private readonly IDatabase _db;

        public UptimePerformanceIndicatorsController(IDatabase db)
        {
            _db = db;
        }

        private static string GetSQL( UptimePerformanceIndicatorsArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "tx.req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "tx.req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.MerchantIds, "tx.mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.CorporateIds, "tx.corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.BankIds, "tx.mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.MID, "tx.mid = (@MID)");

            var where = criteria.ToWhereClause();
            var groupBy = args.GroupByBankId.HasValue && args.GroupByBankId.Value ? "tx.mongo_bank_id, tx.bank_name, tx.mid" : "tx.mid, tx.mongo_bank_id, tx.bank_name";
            var orderBy = args.OrderBys.ToOrderByClause("");

            var sql = $@"SELECT 
                    tx.mid AS MID, 
                    tx.global_id AS GlobalId, 
                    tx.global_name AS Global, 
                    tx.mongo_bank_id AS BankId, 
                    tx.bank_name AS Bank, 
                    tx.mongo_m_id AS MerchantId, 
                    tx.corp_id AS CorporateId, 
                    tx.corp_name AS Corporate, 
                    round(avg ((DATE_PART('day', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw) * 24*60*60  +
                    DATE_PART('hour', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw) * 3600  +
                    DATE_PART('minute', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw) * 60 +
                    DATE_PART('second', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw))):: numeric,2) AS TotalDurationTransaction,
                    round(avg(ts.ext_Apis_Total)::numeric, 3) AS TotalDurationProviders,
                    round( ((avg ((DATE_PART('day', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw) * 24*60*60  +
                    DATE_PART('hour', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw) * 3600  +
                    DATE_PART('minute', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw) * 60 +
                    DATE_PART('second', tx.res_snt_back_to_mrchnt - tx.req_rcv_at_main_flw)))) - avg(ts.ext_Apis_Total)):: numeric, 3) as TotalDurationAPC
                    FROM cs_rpt_txn tx LEFT JOIN
                    (SELECT cs_rpt_txn_id,
                       SUM((DATE_PART('day', res_rcv_frm_ext_api - req_snt_to_ext_api) * 24*60*60  +
                            DATE_PART('hour', res_rcv_frm_ext_api - req_snt_to_ext_api) *3600  +
                            DATE_PART('minute', res_rcv_frm_ext_api - req_snt_to_ext_api) * 60 +
                            DATE_PART('second', res_rcv_frm_ext_api - req_snt_to_ext_api))
                    ) AS ext_Apis_Total    
                    FROM cs_rpt_timestamp GROUP BY cs_rpt_txn_id) ts
                    ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                    {where} 
                    GROUP BY {groupBy}, tx.mongo_m_id, tx.corp_id, tx.corp_name, tx.global_id, tx.global_name
                    {orderBy}";

            return sql;
        }

        [HttpGet]
        public PagedResult<UptimePerformanceIndicatorsRow> Get([FromQuery]UptimePerformanceIndicatorsArgs args)
        {
            var sql = GetSQL(args);
            var result = _db.FetchPagedResult<UptimePerformanceIndicatorsRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]UptimePerformanceIndicatorsArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var sql = GetSQL(args).ToOptionsSQL(optionsArgs);
            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
