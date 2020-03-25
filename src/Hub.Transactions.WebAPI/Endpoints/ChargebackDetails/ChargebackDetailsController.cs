using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.ChargebackDetails
{
    [Route("[controller]")]
    public class ChargebackDetailsController : Controller
    {
        private readonly IDatabase _db;

        public ChargebackDetailsController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<ChargebackDetailsRow> Get([FromQuery]ChargebackDetailsArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.IDs, "ts.id IN (@IDs)");

            var where = criteria.ToWhereClause();


            var sql = @"select record_type as RecordType, 
                        date as Date, 
                        addressee as Addressee, 
                        case_number as CaseNumber, 
                        card_number as CardNumber, 
                        transaction_date as TransactionDate, 
                        transaction_amount as TransactionAmount, 
                        currency_code as CurrencyCode, 
                        settlement_amount as SettlementAmount, 
                        settlement_currency as SettlementCurrency, 
                        transaction_type as TypeOfTransaction, 
                        merchant_name as MerchantName, 
                        merchant_location as MerchantLocation, 
                        merchant_country as MerchantCountry, 
                        merchant_store_number as MerchantStoreNumber, 
                        auth_id as AuthCode, 
                        comment as Comment, 
                        additional_info as AdditionalInfo, 
                        rrn As RRN, 
                        terminal_id as TerminalID, 
                        merchant_id as MerchantID, 
                        related_refs as RelatedRefs, 
                        acquired_ref as AcquirerRef
                        from batch_chargeback_rpt bcr LEFT JOIN cs_rpt_timestamp ts ON bcr.chargeback_txn_id = ts.cs_rpt_txn_id 
                        " + where;

            var result = _db.FetchPagedResult<ChargebackDetailsRow>(args.Page, args.PageSize, sql, args);

            return result;
        }
    }
}

