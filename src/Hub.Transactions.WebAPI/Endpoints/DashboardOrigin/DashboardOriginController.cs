using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.DashboardOrigin
{
    [Route("[controller]")]
    public class DashboardOriginController : Controller
    {
        private readonly IDatabase _db;

        public DashboardOriginController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<DashboardOriginRow> Get([FromQuery]DashboardOriginArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("resp_code IN ('1000','1042')");
            criteria.Add("txn_type IN ('CapturePayment','OnlinePayment','OneStepPayment')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");

            var where = criteria.ToWhereClause();

            var sql = $@"select 
                 (case when requestor_origin is null or requestor_origin = '' then 'Unknown' else requestor_origin end) as Origin, 
                 count(*) as Count 
                 from cs_rpt_txn 
                 {where}
                 group by Origin";

            var result = _db.FetchPagedResult<DashboardOriginRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
