using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;
using System.Linq;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionsEligibleForRefund
{
    [Route("[controller]")]
    public class TransactionsEligibleForRefundController
    {
        private readonly IDatabase _db;

        public TransactionsEligibleForRefundController(IDatabase db)
        {
            _db = db;
        }

        private static string GetSQL(TransactionsEligibleForRefundArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
            criteria.AddIfNotNull(args.DateFrom, "tx.req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "tx.req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "tx.mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "tx.corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "tx.mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.MerchantTransactionID, "tx.m_txn_id LIKE @MerchantTransactionID", () => args.MerchantTransactionID = string.Format("%{0}%", args.MerchantTransactionID));
            criteria.AddIfNotNull(args.Currencies, "tx.currency IN (@Currencies)");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "tx.exch_currency IN (@ExchangeCurrencies)");
            criteria.AddIfNotNull(args.BIN, "tx.bin LIKE @BIN", () => args.BIN = string.Format("%{0}%", args.BIN));
            criteria.AddIfNotNull(args.APIIds, "ts.api_id IN (@APIIds)");

            var tmpWhere = criteria.ToWhereClause();

            if (!string.IsNullOrEmpty(args.MerchantTransactionID))
            {
                criteria.Add("case " +
                             "when " +
                                "tx.resp_code IN('1000', '1116') AND tx.txn_type IN ('CapturePayment', 'OnlinePayment', 'OneStepPayment') " +
                             "then " +
                                "(tx.amount - COALESCE(tmp.netamt, 0) - COALESCE(tmpCancel.netamt, 0)) > 0 " +
                             "end");
            }
            else
            {
                criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
                criteria.Add("case " +
                             "when " +
                                "tx.resp_code IN('1000', '1116') AND tx.txn_type IN ('CapturePayment', 'OnlinePayment', 'OneStepPayment') " +
                             "then " +
                                "(tx.amount - COALESCE(tmp.netamt, 0) - COALESCE(tmpCancel.netamt, 0)) > 0 " +
                             "end");
            }

            criteria.AddIfNotNull(args.DateFrom, "tx.req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "tx.req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.ResponseCodes, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.TransactionTypes, "tx.txn_type IN (@TransactionTypes)");

            var where = criteria.ToWhereClause();
            var orderBy = args.OrderBys.ToOrderByClause("ID DESC, Date");

            var sql = $@"
                WITH tmp AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    {tmpWhere}
                        AND tx.resp_code IN ('1000', '1116')
                        AND (tx.txn_type = 'RefundPayment' OR (tx.txn_type = 'PreApproveRefund' AND tx.pre_approval_status != 'PROCESSED'))
                    GROUP BY tx.m_txn_id,
                        tx.mongo_m_id),

					 tmpCancel AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    {tmpWhere} 
                        AND tx.resp_code = '1000'
                        AND tx.txn_type = 'CancelPayment'
                    GROUP BY tx.m_txn_id,
                        tx.mongo_m_id)

                SELECT ts.id AS ID, 
                    tx.global_id AS GlobalId, 
                    tx.global_name AS Global, 
                    tx.mongo_bank_id AS BankId, 
                    tx.bank_name AS Bank,
                    tx.corp_id AS CorporateId, 
                    tx.corp_name AS Corporate, 
                    tx.mongo_m_id AS MerchantId,
                    tx.merchant_name AS Merchant,
                    tx.mid AS MID, 
                    ts.api_id AS APIId,
                    ts.api_name AS Product, 
                    tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval AS Date,
                    tx.m_txn_id AS MerchantTransactionID, 
                    tx.amount*.001 AS Amount, 
                    case
                        when (COALESCE(tmp.netamt, 0)) = 0 then (tx.amount)*.001 
                        when (COALESCE(tmp.netamt, 0)) > 0 AND tx.resp_code IN('1000', '1116') AND tx.txn_type IN ('CapturePayment', 'OnlinePayment', 'OneStepPayment')
                            then (tx.amount - COALESCE(tmp.netamt, 0) - COALESCE(tmpCancel.netamt, 0))*.001 
                    end as AmountEligibleForRefund,
                    tx.currency AS Currency,
                    tx.bin AS BIN, 
                    tx.acc_holder AS AccountHolder,
                    tx.crypto_key AS CryptoId, 
                    case when tx.exch_rate > '' then cast(tx.exch_rate as numeric) else 0 end AS ExchangeRate, 
                    case when tx.exch_amount > '' then cast(tx.exch_amount as numeric) * .001 else 0 end AS ExchangeAmount, 
                    tx.exch_currency AS ExchangeCurrency,
                    tx.txn_type AS TransactionType,
                    tx.resp_code AS ResponseCode, 
                    tx.resp_code_desc AS ResponseCodeDescription
                FROM cs_rpt_txn tx
                    LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    LEFT JOIN tmp ON tmp.m_txn_id = tx.m_txn_id AND tmp.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpCancel ON tmpCancel.m_txn_id = tx.m_txn_id AND tmpCancel.mongo_m_id = tx.mongo_m_id
                {where}
                {orderBy}";

            return sql;
        }

        [HttpGet]
        public PagedResult<TransactionsEligibleForRefundRow> Get([FromQuery] TransactionsEligibleForRefundArgs args)
        {
            var sql = GetSQL(args);
            var transactions = PageExtensions.FetchPagedResult<TransactionsEligibleForRefundRow>(_db, args.Page, args.PageSize, sql, args);

            return transactions;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery] TransactionsEligibleForRefundArgs args, [FromQuery] OptionArgs optionsArgs)
        {
            var sql = GetSQL(args).ToOptionsSQL(optionsArgs);
            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
