using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.AuthResponseReason
{
    [Route("[controller]")]
    public class AuthResponseReasonController : Controller
    {
        private readonly IDatabase _db;

        public AuthResponseReasonController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<AuthResponseReasonRow> Get([FromQuery]AuthResponseReasonArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','OneStepPayment','VerifyAccount')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var with = @"WITH tmp_table AS (select mid, (req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date as date, card_type, Bin, count(*) as total_count from cs_rpt_txn tx
                        " + where + @" 
                        GROUP BY mid, date, card_type, Bin ORDER BY mid, date, card_type, Bin) ";

            var sql = with + @"SELECT 
                                tx.global_name AS Global, 
                                tx.global_id AS GlobalId, 
                                tx.bank_name AS Bank, 
                                tx.mongo_bank_id AS BankId, 
                                tx.corp_name AS Corporate, 
                                tx.corp_id AS CorporateId, 
                                tx.merchant_name AS Merchant, 
                                tx.mongo_m_id AS MerchantId, 
                                tx.mid AS MID, 
                                (tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval)::date AS TransactionDate, 
                                case when tx.card_type is null or tx.card_type = '' then '[unknown]' else tx.card_type end AS CardType, 
                                COALESCE(NULLIF(tx.Bin,''), '[unknown]') AS BIN, 
                                COALESCE(NULLIF(tx.resp_code,''), '[unknown]') AS ResponseCode, 
                                tx.resp_code_desc AS ResponseDescription, 
                                count(*) AS Count, 
                                round(count(*)/tmp.total_count::decimal, 2) * 100 AS ResponseCodesPercentage 
                                FROM cs_rpt_txn tx 
                                LEFT JOIN tmp_table tmp ON tmp.mid = tx.mid AND tmp.date = tx.req_rcv_at_main_flw::date AND tmp.card_type = tx.card_type AND tmp.Bin = tx.Bin
                                " + where + @" 
                                GROUP BY tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.corp_name, tx.corp_id, tx.merchant_name, tx.mongo_m_id, tx.mid, TransactionDate, tx.card_type, tx.Bin, tx.resp_code, tx.resp_code_desc, tmp.total_count 
                                ORDER BY tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.corp_name, tx.corp_id, tx.merchant_name, tx.mongo_m_id, tx.mid, TransactionDate, tx.card_type, tx.Bin, tx.resp_code, tx.resp_code_desc, tmp.total_count";

            var result = _db.FetchPagedResult<AuthResponseReasonRow>(args.Page, args.PageSize, sql, args);

            return result;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]AuthResponseReasonArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var criteria = new List<string>();

            criteria.Add("tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','VerifyAccount')");
            criteria.AddIfNotNull(args.DateFrom, "req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "GlobalId", optionsArgs.OptionValue, "global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "BankId", optionsArgs.OptionValue, "mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "CorporateId", optionsArgs.OptionValue, "corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "MerchantId", optionsArgs.OptionValue, "mongo_m_id = (@MerchantIds)");

            var where = criteria.ToWhereClause();

            var with = @"WITH tmp_table AS (select mid, req_rcv_at_main_flw::date as date, card_type, Bin, count(*) as total_count from cs_rpt_txn tx
                        " + where + @" 
                        GROUP BY mid, req_rcv_at_main_flw::date, card_type, Bin ORDER BY mid, req_rcv_at_main_flw::date, card_type, Bin) ";

            var sql = with + @"SELECT DISTINCT 
                                " + optionsArgs.OptionValue.MapToColumn() + @" AS Key, 
                                " + optionsArgs.OptionText.MapToColumn() + @" AS Value 
                                FROM cs_rpt_txn tx 
                                LEFT JOIN tmp_table tmp ON tmp.mid = tx.mid AND tmp.date = tx.req_rcv_at_main_flw::date AND tmp.card_type = tx.card_type AND tmp.Bin = tx.Bin
                                " + where + @" 
                                GROUP BY tx.global_name, tx.global_id, tx.bank_name, tx.mongo_bank_id, tx.corp_name, tx.corp_id, tx.merchant_name, tx.mongo_m_id, tx.mid, tx.req_rcv_at_main_flw::date, tx.card_type, tx.Bin, tx.resp_code, tx.resp_code_desc, tmp.total_count";

            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
