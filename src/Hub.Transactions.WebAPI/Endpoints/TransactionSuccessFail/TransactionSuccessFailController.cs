using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionSuccessFail
{
    [Route("[controller]")]
    public class TransactionSuccessFailController : Controller
    {
        private readonly IDatabase _db;

        public TransactionSuccessFailController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<TransactionSuccessFailRow> Get([FromQuery]TransactionSuccessFailArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.APIIDs, "ts.api_id IN (@APIIDs)");

            var where = criteria.ToWhereClause();

            var sql = $@"select ts.api_name as Product, 
                        ts.api_id as ProductId, 
                        tx.mongo_m_id as MerchantId, 
                        tx.mid as MID, 
                        tx.corp_id as CorporateId, 
                        tx.corp_name as Corporate, 
                        tx.txn_type as TransactionType, 
                        tx.method_name as PaymentMethod, 
                        (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS Date, 
                        card_type as CardType, 
                        sum(case when tx.resp_code = '1000' then 1 else 0 end) as SuccessCount,  
                        sum(case when tx.resp_code <> '1000' then 1 else 0 end) as FailCount from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        {where}
                        and tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','CapturePayment','OnlinePayment','CreditPayment','VerifyAccount')
                        group by ts.api_name, ts.api_id, tx.mongo_m_id, tx.mid, tx.corp_id, tx.corp_name, tx.txn_type, tx.method_name, Date, card_type
                        order by ts.api_name, ts.api_id, tx.mongo_m_id, tx.mid, tx.corp_id, tx.corp_name, tx.txn_type, tx.method_name, Date, card_type";

            var result = _db.FetchPagedResult<TransactionSuccessFailRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]TransactionSuccessFailArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id = (@MerchantIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.APIIDs, "APIId", optionsArgs.OptionValue, "ts.api_id IN (@APIIDs)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        and tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','CapturePayment','OnlinePayment','CreditPayment','VerifyAccount')
                        group by ts.api_name, ts.api_id, tx.mongo_m_id, tx.mid, tx.corp_id, tx.corp_name, tx.txn_type, tx.method_name, req_rcv_at_main_flw::date, card_type";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
