using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionVolume
{
    [Route("[controller]")]
    public class TransactionVolumeController : Controller
    {
        private readonly IDatabase _db;

        public TransactionVolumeController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<TransactionVolumeRow> Get([FromQuery]TransactionVolumeArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("amount > 0");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DATE_PART('hour', req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::integer as Hour, 
                        count(*) as Count
                        FROM cs_rpt_txn 
                        " + where + @" 
                        GROUP BY Hour
                        ORDER BY Hour";

            var result = _db.FetchPagedResult<TransactionVolumeRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
