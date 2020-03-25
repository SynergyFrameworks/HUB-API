using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.BillingSummary
{
    [Route("[controller]")]
    public class BillingSummaryController : Controller
    {
        private readonly IDatabase _db;

        public BillingSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<BillingSummaryRow> Get([FromQuery]BillingSummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("mid <> ''");
            criteria.Add("NOT mongo_bank_id = 'UnknownSellingBankId'");
            criteria.Add("mongo_bank_id <> ''");
            criteria.Add("NOT mongo_m_id = 'UnknownMerchantID'");
            criteria.Add("mongo_m_id <> ''");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.APIIds, "ts.api_id IN (@APIIds)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT 
                        tx.global_id AS GlobalId, 
	                    tx.global_name AS Global, 
                        tx.mongo_bank_id as BankId, 
                        tx.bank_name as Bank, 
                        tx.corp_id as CorporateId, 
                        tx.corp_name as Corporate, 
                        tx.mongo_m_id as MerchantId, 
                        tx.mid as MID, 
                        tx.merchant_name as Merchant, 
                        ts.api_id as APIId, 
                        ts.api_name as Product, 
                        count(*) as Count 
                        FROM cs_rpt_txn tx 
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        GROUP BY tx.global_id, tx.global_name, tx.mongo_bank_id, tx.bank_name, tx.corp_id, tx.corp_name, tx.mongo_m_id, tx.mid, tx.merchant_name, ts.api_id, ts.api_name";

            var result = _db.FetchPagedResult<BillingSummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]BillingSummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("mid <> ''");
            criteria.Add("NOT mongo_bank_id = 'UnknownSellingBankId'");
            criteria.Add("mongo_bank_id <> ''");
            criteria.Add("NOT mongo_m_id = 'UnknownMerchantID'");
            criteria.Add("mongo_m_id <> ''");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.APIIds, "APIId", optionsArgs.OptionValue, "ts.api_id IN (@APIIds)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        FROM cs_rpt_txn tx 
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        GROUP BY tx.global_id, tx.global_name, tx.mongo_bank_id, tx.bank_name, tx.corp_id, tx.corp_name, tx.mongo_m_id, tx.mid, tx.merchant_name, ts.api_id, ts.api_name";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
