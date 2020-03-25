using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionSummary
{
    [Route("[controller]")]
    public class TransactionSummaryController : Controller
    {
        private readonly IDatabase _db;

        public TransactionSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<TransactionSummaryRow> Get([FromQuery]TransactionSummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("amount > 0");
            criteria.Add("ts.api_id <> ''");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.APIIds, "ts.api_id IN (@APIIds)");
            criteria.AddIfNotNull(args.Currencies, "currency IN (@Currencies)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT ts.api_id as APIId, 
                        ts.api_name as Product, 
                        currency as Currency, 
                        tx.corp_id, 
                        tx.corp_name, 
                        max(req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval) as LastDate, 
                        round(avg(amount)*.001,3) as AverageAmount,
                        round(sum(case when tx.resp_code = '1000' then 1 else 0 end)/count(tx.cs_rpt_txn_id)::decimal, 3) as SuccessRate,
                        Count(*) as TotalVolume,
                        cast(round(avg(((DATE_PART('day', ts.req_snt_to_ext_api::timestamp - ts.res_rcv_frm_ext_api::timestamp) * 24 + 
                                    DATE_PART('hour', ts.res_rcv_frm_ext_api::timestamp - ts.req_snt_to_ext_api::timestamp)) * 60  +
                                    DATE_PART('minute', ts.res_rcv_frm_ext_api::timestamp - ts.req_snt_to_ext_api::timestamp)) * 60 +
                                    DATE_PART('second', ts.res_rcv_frm_ext_api::timestamp - ts.req_snt_to_ext_api::timestamp)):: numeric, 1) as numeric) as AverageSeconds, 
                        case when count(distinct(bin_country)) > 0 then count(distinct(bin_country)) else 1 end as TotalCountry 
                        FROM cs_rpt_txn tx 
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                        " + where + @" 
                        GROUP BY ts.api_id, ts.api_name, currency, tx.corp_id, tx.corp_name
                        ORDER BY ts.api_name";

            var result = _db.FetchPagedResult<TransactionSummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
