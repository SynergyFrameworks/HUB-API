using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.ThreeDSecureDetails
{
    [Route("[controller]")]
    public class ThreeDSecureDetailsController : Controller
    {
        private readonly IDatabase _db;

        public ThreeDSecureDetailsController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<ThreeDSecureDetailsRow> Get([FromQuery]ThreeDSecureDetailsArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.IDs, "ts.id IN (@IDs)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT enrolled AS Enrolled, 
                        authenticated AS Authenticated, 
                        eci AS ECI, 
                        xid AS XID, 
                        authentication_value AS AuthenticationValue, 
                        pareq AS PAReq, 
                        pares AS PARes, 
                        vereq AS VEReq, 
                        veres AS VERes 
                        FROM threed_secure_result tds LEFT JOIN cs_rpt_timestamp ts ON tds.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                        " + where;

            var result = _db.FetchPagedResult<ThreeDSecureDetailsRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
