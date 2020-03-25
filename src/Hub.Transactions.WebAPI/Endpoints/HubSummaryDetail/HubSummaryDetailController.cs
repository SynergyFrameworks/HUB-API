using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.HubSummaryDetail
{
    [Route("[controller]")]
    public class HubSummaryDetailController : Controller
    {
        private readonly IDatabase _db;

        public HubSummaryDetailController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<HubSummaryDetailRow> Get([FromQuery]HubSummaryDetailArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("mid <> ''");
            criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.TransactionTypes, "txn_type IN (@TransactionTypes)");

            var where = criteria.ToWhereClause();

            var sql = @"select ts.api_name as Product, 
                        tx.corp_id as CorporateId, 
                        tx.corp_name as Corporate, 
                        Count(*) as Count
                        from cs_rpt_txn tx 
				        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                " + where + @"
		                group by ts.api_name, tx.corp_id, tx.corp_name
		                order by ts.api_name, tx.corp_id, tx.corp_name";

            var result = _db.FetchPagedResult<HubSummaryDetailRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]HubSummaryDetailArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("mid <> ''");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.TransactionTypes, "txn_type IN (@TransactionTypes)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx 
				        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                " + where + @"
		                group by ts.api_name, tx.corp_id, tx.corp_name";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
