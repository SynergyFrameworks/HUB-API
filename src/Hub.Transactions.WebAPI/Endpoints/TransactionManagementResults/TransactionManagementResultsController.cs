using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionManagementResults
{
    [Route("[controller]")]
    public class TransactionManagementResultsController : Controller
    {
        private readonly IDatabase _db;

        public TransactionManagementResultsController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public List<TransactionManagementResultsRow> Get([FromQuery] TransactionManagementResultsArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("ts.id IN (@IDs)");

            var where = criteria.ToWhereClause();

            var sql = $@"SELECT
                    ts.id as ID,
                    tx.cs_rpt_txn_id as TransactionID,
                    tx.userid as UserID,
                    tx.requestor_id as RequestorID,
                    tx.global_id AS GlobalId, 
                    tx.bank_id as BankID,
                    tx.mid as MID,
                    tx.apc_txn_id as APCTransactionID,
                    tx.m_txn_id as MerchantTxnID,
                    ts.api_id as APIId, 
                    ts.api_name as Product,
                    tx.txn_type as TransactionType,
                    tx.method_name as PaymentMethod,
                    tx.channel_type as ChannelType,
                    tx.currency as Currency,
                    tx.amount as Amount,
                    tx.card_num as CardNumber,
                    tx.acc_holder as CardHolder,
                    tx.exp_month as CardExpiryMonth,
                    tx.exp_year as CardExpiryYear,
                    tx.crypto_key as CryptoId
                FROM cs_rpt_txn tx 
                    LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                {where}
                ORDER BY ID DESC";

            var result = _db.Fetch<TransactionManagementResultsRow>(sql, args);

            return result;
        }
    }
}
