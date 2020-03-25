using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.DashboardPaymentMethod
{
    [Route("[controller]")]
    public class DashboardPaymentMethodController : Controller
    {
        private readonly IDatabase _db;

        public DashboardPaymentMethodController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<DashboardPaymentMethodRow> Get([FromQuery]DashboardPaymentMethodArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("resp_code = '1000'");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");

            var where = criteria.ToWhereClause();

            var sql = @"select 
                 (case when method_name is null or method_name = '' then 'Unknown' else method_name end) as PaymentMethod, 
                 count(*) as Count 
                 from cs_rpt_txn 
                 " + where + @" 
                 group by PaymentMethod";

            var result = _db.FetchPagedResult<DashboardPaymentMethodRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
