using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionByCountrySummary
{
    [Route("[controller]")]
    public class TransactionByCountrySummaryController : Controller
    {
        private readonly IDatabase _db;

        public TransactionByCountrySummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<TransactionByCountrySummaryRow> Get([FromQuery]TransactionByCountrySummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MID, "mid = (@MID)");
            criteria.AddIfNotNull(args.Country, "upper(tx.billing_address_country) = @Country", () => args.Country = args.Country.ToUpper());
            criteria.AddIfNotNull(args.PaymentMethod, "method_name = @PaymentMethod");

            var where = criteria.ToWhereClause();

            var sql = $@"select 
                        ts.api_name as Product, 
                        mid as MID, 
                        tx.corp_id AS CorporateId, 
                        tx.corp_name AS Corporate, 
                        tx.billing_address_country as Country, 
                        (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS Date, 
                        tx.card_type as CardType, 
                        tx.method_name as PaymentMethod, 
                        sum(case when tx.resp_code = '1000' then 1 else 0 end) as SuccessCount,  
                        sum(case when tx.resp_code <> '1000' then 1 else 0 end) as FailCount 
                        from cs_rpt_txn tx
		                LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                {where}
		                and tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','CapturePayment','OnlinePayment','CreditPayment')
		                group by ts.api_name, mid, tx.corp_id, tx.corp_name, tx.method_name, Date, card_type, billing_address_country
		                order by ts.api_name, mid, tx.corp_id, tx.corp_name, tx.method_name, Date, card_type, billing_address_country";

            var result = _db.FetchPagedResult<TransactionByCountrySummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]TransactionByCountrySummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MID, "MID", optionsArgs.OptionValue, "mid = (@MID)");
            criteria.AddIfNotNull(args.Country, "Country", optionsArgs.OptionValue, "upper(tx.billing_address_country) = @Country", () => args.Country = args.Country.ToUpper());
            criteria.AddIfNotNull(args.PaymentMethod, "PaymentMethod", optionsArgs.OptionValue, "method_name = @PaymentMethod");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx
		                LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
		                " + where + @"
		                and tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','CapturePayment','OnlinePayment','CreditPayment')
		                group by api_name, mid, tx.corp_id, tx.corp_name, tx.method_name, req_rcv_at_main_flw::date, card_type, billing_address_country";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
