using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.PointsInstallmentsDetails
{
    [Route("[controller]")]
    public class PointsInstallmentsDetailsController : Controller
    {
        private readonly IDatabase _db;

        public PointsInstallmentsDetailsController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<PointsInstallmentsDetailsRow> Get([FromQuery]PointsInstallmentsDetailsArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.IDs, "ts.id IN (@IDs)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT cardbin_pay_points AS Points, 
                        installment_type AS InstallmentType,
                        pay_number_months AS PayNumberMonths, 
                        pay_later_months AS PayLaterMonths 
                        FROM cs_rpt_txn tx LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                        " + where;

            var result = _db.FetchPagedResult<PointsInstallmentsDetailsRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
