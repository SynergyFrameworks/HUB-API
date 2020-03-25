using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionChart
{
    [Route("[controller]")]
    public class TransactionChartController : Controller
    {
        private readonly IDatabase _db;

        public TransactionChartController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<TransactionChartRow> Get([FromQuery]TransactionChartArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("txn_type <> ''");
            criteria.AddIfNotNull(args.MID, "mid = @MID");

            var where = criteria.ToWhereClause();

            var sql = @"select txn_type as TransactionType, 
                        extract(dow from (req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::timestamp) as DayNo, 
		                case extract(dow from (req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::timestamp)
                        when 1 then 'Monday'
                        when 2 then 'Tuesday'
                        when 3 then 'Wednesday'
                        when 4 then 'Thursday'
                        when 5 then 'Friday'
                        when 6 then 'Saturday'
			            else 'Sunday'
                        END as Day, 
		                count(*) as Count
                        from cs_rpt_txn tx
                        " + where + @"
                        GROUP BY 1, 2, 3
                        ORDER BY 1";

            var result = _db.FetchPagedResult<TransactionChartRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
