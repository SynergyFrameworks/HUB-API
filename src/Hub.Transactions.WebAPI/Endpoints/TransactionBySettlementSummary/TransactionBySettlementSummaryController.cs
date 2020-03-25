using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionBySettlementSummary
{
    [Route("[controller]")]
    public class TransactionBySettlementSummaryController : Controller
    {
        private readonly IDatabase _db;

        public TransactionBySettlementSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<TransactionBySettlementSummaryRow> Get([FromQuery]TransactionBySettlementSummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("txn_type IN ('OneStepPayment', 'RefundPayment', 'CapturePayment', 'CancelPayment')");
            criteria.AddIfNotNull(args.DateFrom, "settlement_date >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "settlement_date <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "currency IN (@Currencies)");

            var where = criteria.ToWhereClause();

            var sql = @"select 
                        tx.settlement_date::timestamp without time zone + (@TimeZoneOffset ||' minutes')::interval AS Date, 
                        tx.global_id AS GlobalId, 
                        tx.global_name AS Global, 
                        tx.mongo_bank_id as BankId, 
                        tx.bank_name as Bank, 
                        tx.corp_id as CorporateId, 
                        tx.corp_name as Corporate,
                        tx.mongo_m_id as MerchantId, 
                        tx.mid as MID, 
                        tx.currency as Currency,
                        sum(case when tx.txn_type = 'CancelPayment' or tx.txn_type = 'RefundPayment' then tx.amount else 0 end) as DebitTotal, 
                        sum(case when tx.txn_type = 'OneStepPayment' or tx.txn_type = 'CapturePayment' then tx.amount else 0 end) as CreditTotal
                        from cs_rpt_txn tx 
                        " + where + @"
                        group by tx.global_id, tx.global_name, tx.mongo_bank_id, tx.bank_name, tx.corp_id, tx.corp_name, tx.mongo_m_id, tx.mid, Date, tx.currency
                        order by Date, tx.currency";

            var result = _db.FetchPagedResult<TransactionBySettlementSummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]TransactionBySettlementSummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("txn_type IN ('OneStepPayment', 'RefundPayment', 'CapturePayment', 'CancelPayment')");
            criteria.AddIfNotNull(args.DateFrom, "settlement_date >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "settlement_date <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "Currency", optionsArgs.OptionValue, "currency IN (@Currencies)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT  
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx 
                        " + where + @"
                        group by tx.settlement_date, tx.currency";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
