﻿using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.AuthorisationSummary
{
    [Route("[controller]")]
    public class AuthorisationSummaryController : Controller
    {
        private readonly IDatabase _db;

        public AuthorisationSummaryController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<AuthorisationSummaryRow> Get([FromQuery]AuthorisationSummaryArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MID, "mid = (@MID)");

            var where = criteria.ToWhereClause();

            var sql = $@"select 
                        ts.api_name as Product, 
                        tx.corp_id AS CorporateId, 
                        tx.corp_name AS Corporate, 
                        COALESCE(NULLIF(mid,''), '[unknown]') AS MID, 
                        (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS Date, 
                        COALESCE(NULLIF(card_type,''), '[unknown]') AS CardType, 
                        sum(case when tx.resp_code = '1000' then 1 else 0 end) as AuthCount,  
                        sum(case when tx.resp_code <> '1000' then 1 else 0 end) as DeclineCount 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        {where}
                        and tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','VerifyAccount')
                        group by api_name, tx.corp_id, tx.corp_name, mid, Date, card_type
                        order by api_name, tx.corp_id, tx.corp_name, mid, Date, card_type";

            var result = _db.FetchPagedResult<AuthorisationSummaryRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]AuthorisationSummaryArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MID, "MID", optionsArgs.OptionValue, "mid = (@MID)");

            var where = criteria.ToWhereClause();

            var sql = @"SELECT DISTINCT 
                        " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                        " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                        from cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        " + where + @"
                        and tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','VerifyAccount')
                        group by api_name, tx.corp_id, tx.corp_name, mid, req_rcv_at_main_flw::date, card_type";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
