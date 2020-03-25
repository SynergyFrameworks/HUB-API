using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.APIResponseTimes
{
    [Route("[controller]")]
    public class APIResponseTimesController
    {
        private readonly IDatabase _db;

        public APIResponseTimesController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<APIResponseTimesRow> Get([FromQuery] APIResponseTimesArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.amount > 0");
            criteria.AddIfNotNull(args.DateFrom, "tx.req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "tx.req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "tx.mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "tx.corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "tx.mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var groupBy = args.GroupByBankId ? "tx.mongo_bank_id, tx.bank_name, tx.mid" : "tx.mid, tx.mongo_bank_id, tx.bank_name";

            var sql = $@"SELECT tx.mid AS MID, 
                    tx.global_id AS GlobalId, 
	                tx.global_name AS Global, 
	                tx.mongo_bank_id AS BankId, 
	                tx.bank_name AS Bank, 
	                tx.mongo_m_id AS MerchantId, 
	                tx.corp_id AS CorporateId, 
	                tx.corp_name AS Corporate, 
	                round(avg((DATE_PART('day', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval) * 24*60*60  +
		                DATE_PART('hour', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval) * 3600  +
		                DATE_PART('minute', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval) * 60 +
		                DATE_PART('second', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval))):: numeric,2) as TotalDurationTransaction,
	                round(avg(ts.ext_Apis_Total)::numeric, 3) as TotalDurationProviders,
	                round(((avg((DATE_PART('day', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval) * 24*60*60  +
		                DATE_PART('hour', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval) * 3600  +
		                DATE_PART('minute', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval) * 60 +
		                DATE_PART('second', tx.res_snt_back_to_mrchnt + (@TimeZoneOffset ||' minutes')::interval - tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)))) - avg(ts.ext_Apis_Total)):: numeric, 3) as TotalDurationAPC,  
	                count(tx.cs_rpt_txn_id) as NumberOfRequests
                FROM cs_rpt_txn tx 
	                LEFT JOIN (SELECT tsi.cs_rpt_txn_id,
		                    SUM((DATE_PART('day', tsi.res_rcv_frm_ext_api + (@TimeZoneOffset ||' minutes')::interval - tsi.req_snt_to_ext_api + (@TimeZoneOffset ||' minutes')::interval) * 24*60*60  +
		                        DATE_PART('hour', tsi.res_rcv_frm_ext_api + (@TimeZoneOffset ||' minutes')::interval - tsi.req_snt_to_ext_api + (@TimeZoneOffset ||' minutes')::interval) * 3600  +
		                        DATE_PART('minute', tsi.res_rcv_frm_ext_api + (@TimeZoneOffset ||' minutes')::interval - tsi.req_snt_to_ext_api + (@TimeZoneOffset ||' minutes')::interval) * 60 +
		                        DATE_PART('second', tsi.res_rcv_frm_ext_api + (@TimeZoneOffset ||' minutes')::interval - tsi.req_snt_to_ext_api + (@TimeZoneOffset ||' minutes')::interval))) AS ext_Apis_Total
		                FROM cs_rpt_timestamp tsi
		                GROUP BY tsi.cs_rpt_txn_id) ts
			                ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                {where}
                GROUP BY {groupBy}, tx.mongo_m_id, tx.corp_id, tx.corp_name, tx.global_id, tx.global_name";

            var transactions = PageExtensions.FetchPagedResult<APIResponseTimesRow>(_db, args.Page, args.PageSize, sql, args);

            return transactions;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery] APIResponseTimesArgs args, [FromQuery] OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("tx.amount > 0");
            criteria.AddIfNotNull(args.DateFrom, "tx.req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "tx.req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "tx.mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "tx.corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "tx.mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var valueColumn = optionsArgs.OptionValue.MapToColumn();
            var textColumn = optionsArgs.OptionText.MapToColumn();

            var sql = $@"SELECT DISTINCT
                    {valueColumn} AS Key,
                    {textColumn} AS Value
                FROM cs_rpt_txn tx 
                    LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                {where}
                    AND {valueColumn} <> '' 
                    AND {textColumn} <> ''
                ORDER BY {textColumn}";

            var options = _db.Fetch<FilterOption>(sql, args);

            return options;
        }
    }
}
