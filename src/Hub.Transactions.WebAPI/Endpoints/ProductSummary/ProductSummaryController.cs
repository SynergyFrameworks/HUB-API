using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.ProductSummary
{
    [Route("[controller]")]
    public class ProductSummaryController : Controller
    {
        private readonly IDatabase _db;

        public ProductSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<ProductSummaryRow> Get([FromQuery]ProductSummaryArgs args)
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

            var sql = @"select 
                        tx.global_id AS GlobalId, 
                        tx.global_name AS Global, 
                        mongo_bank_id as BankId, 
                        bank_name as Bank, 
                        tx.corp_id as CorporateId, 
                        tx.corp_name as Corporate, 
                        mongo_m_id as MerchantId, 
                        mid as MID, 
                        txn_type as TransactionType, 
                        Count(*) as Count, 
                        api_name as Product, 
                        case when tx.method_name is null or tx.method_name = '' then tx.card_type else tx.method_name end as PaymentMethod                        
		                from cs_rpt_txn tx 
		                LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                " + where + @"
		                group by tx.global_name, tx.global_id, mongo_bank_id, mongo_m_id, bank_name, tx.corp_id, tx.corp_name, mid, txn_type, api_name, method_name,tx.card_type
		                order by mid desc, txn_type, api_name, method_name,tx.card_type";

            var result = _db.FetchPagedResult<ProductSummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]ProductSummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("mid <> ''");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.TransactionTypes, "TransactionType", optionsArgs.OptionValue, "txn_type IN (@TransactionTypes)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
		                from cs_rpt_txn tx 
		                LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                " + where + @"
		                group by tx.global_name, tx.global_id, mongo_bank_id, mongo_m_id, bank_name, tx.corp_id, tx.corp_name, mid, txn_type, api_name, method_name";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
