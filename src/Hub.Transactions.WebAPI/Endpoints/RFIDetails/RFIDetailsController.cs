using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.RFIDetails
{
    [Route("[controller]")]
    public class RFIDetailsController : Controller
    {
        private readonly IDatabase _db;

        public RFIDetailsController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<RFIDetailsRow> Get([FromQuery]RFIDetailsArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.IDs, "ts.id IN (@IDs)");

            var where = criteria.ToWhereClause();

            var sql = @"select record_type as RecordType, 
                        rfi_requested_date as RFIRequestedDate, 
                        deadline_date as DeadlineDate, 
                        addressee_name as Addressee , 
                        nab_reference as CaseNumber, 
                        card_number as CardNumber, 
                        card_expiry as CardExpiry, 
                        ac_ref as AcRef, 
                        transaction_date as TransactionDate, 
                        transaction_time as TransactionTime, 
                        transaction_amount as TransactionAmount, 
                        currency_code as Currency, 
                        transaction_type as TypeOfTransaction, 
                        merchant_name as MerchantName, 
                        merchant_location as MerchantLocation, 
                        merchant_country as MerchantCountry, 
                        merchant_store_number as MerchantStoreNumber, 
                        eb_number_terminal_id as FacilityNrAndTerminal, 
                        auth_id as AuthCode, 
                        rrn as RRN
                        from batch_rfi_rpt brr LEFT JOIN cs_rpt_timestamp ts ON brr.rfi_txn_id = ts.cs_rpt_txn_id 
                        " + where;


            var result = _db.FetchPagedResult<RFIDetailsRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}
