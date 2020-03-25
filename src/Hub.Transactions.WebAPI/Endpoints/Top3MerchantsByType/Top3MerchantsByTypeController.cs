using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.Top3MerchantsByType
{
    [Route("[controller]")]
    public class Top3MerchantsByTypeController : Controller
    {
        private readonly IDatabase _db;

        public Top3MerchantsByTypeController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<Top3MerchantsByTypeRow> Get([FromQuery]Top3MerchantsByTypeArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','CancelPayment','CapturePayment','OnlinePayment')");
            criteria.AddIfNotNull(args.MID, "tx.mid = @MID");
            
            var where = criteria.ToWhereClause();

            var sql = @"SELECT tx.mid as MID, 
                        tx.txn_type as TransactionType, 
                        Count(*) as Count 	
                        from cs_rpt_txn tx
                        " + where + @"
                        GROUP BY tx.mid, tx.txn_type
                        ORDER BY tx.mid desc";

            var result = _db.FetchPagedResult<Top3MerchantsByTypeRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
