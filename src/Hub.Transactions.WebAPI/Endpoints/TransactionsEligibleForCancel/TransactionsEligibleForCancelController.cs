using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionsEligibleForCancel
{
    [Route("[controller]")]
    public class TransactionsEligibleForCancelController
    {
        private readonly IDatabase _db;

        public TransactionsEligibleForCancelController(IDatabase db)
        {
            _db = db;
        }

        private static string GetSQL(TransactionsEligibleForCancelArgs args)
        {
            var criteria = new List<string>();

            // ==> The next statement was an attempt to avoid using the "RANK() OVER()" function and having to add one more temporary table. But the method [WebAPI.Extensions.StringExtensions.ToOptionsSQL()] looks for "sql.LastIndexOf(" SELECT ")" and prevents the use of subqueries
            // criteria.Add("NOT EXISTS(SELECT 1 " +
            //             "           FROM cs_rpt_txn tx1 INNER JOIN " +
            //             "                cs_rpt_timestamp ts1 ON tx1.cs_rpt_txn_id = ts1.cs_rpt_txn_id " +
            //             "           WHERE tx1.m_txn_id = tx.m_txn_id AND ts1.id > ts.id )");
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
                        "when "+
                           "(COALESCE(tmpRefund.netamt, 0) > 0 AND (tx.resp_code IN ('1000', '1042') AND tx.txn_type IN('CapturePayment', 'OneStepPayment'))) " +
                        "then (tx.amount - COALESCE(tmpRefund.netamt, 0) - COALESCE(tmp.netamt, 0)) > 0 " +
                        "when "+
                            "(COALESCE(tmpRefund.netamt, 0) = 0 AND (tx.resp_code IN ('1000', '1042') AND tx.txn_type IN('CapturePayment','OneStepPayment'))) " +
                        "then (tx.amount - COALESCE(tmp.netamt, 0)) > 0 " +
                        "when (tx.resp_code = '1000' AND tx.txn_type IN('AuthPayment', 'IncrementalAuthPayment', 'OnlinePayment')) " +
                        "then (tx.amount - COALESCE(tmpCapture.netamt, 0) - COALESCE(tmp.netamt, 0)) > 0 " +
                        "when "+
                        "(tx.resp_code = '1042' AND tx.txn_type IN('RefundPayment')) "+
                        "then(tmpFirstTxn.amount - COALESCE(tmp.netamt, 0)) > 0 "+
                        "end");
            }
            else
            {
                criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
                criteria.Add("tx.resp_code = '1000' AND tx.m_txn_id NOT IN (SELECT tx.m_txn_id FROM tmp tx LEFT JOIN cs_rpt_txn T2 ON tx.m_txn_id = T2.m_txn_id WHERE tx.txn_type IN ('CancelPayment','PreApproveRefund','RefundPayment')) AND tx.txn_type IN ('AuthPayment','IncrementalAuthPayment','CapturePayment','OnlinePayment','OneStepPayment')");
            }
            
            criteria.AddIfNotNull(args.DateFrom, "tx.req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "tx.req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.ResponseCodes, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.TransactionTypes, "tx.txn_type IN (@TransactionTypes)");

            var where = criteria.ToWhereClause();
            var orderBy = args.OrderBys.ToOrderByClause("tx.txn_type DESC") + ")";
            var EndResult = "Select * from tmpFirstTxn1 Where rank_id = 1 Order By MerchantTransactionID, Date DESC";

            var sql = $@"
                WITH tmp AS
                    (SELECT tx.m_txn_id,
                        tx.txn_type,
                        tx.mongo_m_id,
		                COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    {tmpWhere}
                        AND (tx.resp_code = '1000' AND tx.txn_type = 'CancelPayment')
                    GROUP BY tx.m_txn_id,
                        tx.txn_type,
                        tx.mongo_m_id),

                tmpRefund AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    {tmpWhere} 
                        AND tx.resp_code IN ('1000', '1042', '1116')
                        AND (tx.txn_type = 'RefundPayment' OR (tx.txn_type = 'PreApproveRefund' AND tx.pre_approval_status != 'PROCESSED'))
                    GROUP BY tx.m_txn_id,
                        tx.mongo_m_id),

                tmpCapture AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    {tmpWhere} 
                        AND tx.resp_code IN ('1000', '1042', '1116')
                        AND tx.txn_type = 'CapturePayment'
                    GROUP BY tx.m_txn_id,
                        tx.mongo_m_id),

                tmpFirstTxn AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        tx.amount
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    {tmpWhere} 
                        AND tx.resp_code IN('1000', '1042', '1116') AND tx.txn_type IN('AuthPayment', 'OneStepPayment', 'OnlinePayment')
                    order by tx.req_rcv_at_main_flw asc 
                    limit 1), 
        
        tmpFirstTxn1 AS 
                  ( SELECT ts.id AS ID, 
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
                        when (COALESCE(tmpRefund.netamt, 0) > 0 AND tx.txn_type IN('CapturePayment', 'OneStepPayment'))
                            then (tx.amount - COALESCE(tmpRefund.netamt, 0) - COALESCE(tmp.netamt, 0))*.001
                        when (COALESCE(tmpRefund.netamt, 0) = 0 AND tx.txn_type IN('CapturePayment','OneStepPayment'))
                            then (tx.amount - COALESCE(tmp.netamt, 0))*.001
                        when (tx.txn_type IN('AuthPayment', 'IncrementalAuthPayment', 'OnlinePayment'))
                            then (tx.amount - COALESCE(tmpCapture.netamt, 0) - COALESCE(tmp.netamt, 0))*.001
                        when
                            (tx.resp_code = '1042' AND tx.txn_type ='RefundPayment')
                        then (tmpFirstTxn.amount - COALESCE(tmpRefund.netamt, 0) - COALESCE(tmp.netamt, 0))*.001
                    end AS AmountEligibleForCancel,
                    tx.currency AS Currency,
                    tx.bin AS BIN, 
                    tx.acc_holder AS AccountHolder,
                    tx.crypto_key AS CryptoId, 
                    case when tx.exch_rate > '' then cast(tx.exch_rate as numeric) else 0 end AS ExchangeRate, 
                    case when tx.exch_amount > '' then cast(tx.exch_amount as numeric) * .001 else 0 end AS ExchangeAmount, 
                    tx.exch_currency AS ExchangeCurrency,
                    tx.txn_type AS TransactionType,
                    tx.resp_code AS ResponseCode, 
                    tx.resp_code_desc AS ResponseCodeDescription,
                    RANK() OVER(Partition BY tx.m_txn_id ORDER BY tx.m_txn_id , Id DESC) AS rank_id
                FROM cs_rpt_txn tx
                    LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    LEFT JOIN tmp ON tmp.m_txn_id = tx.m_txn_id AND tmp.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpRefund ON tmpRefund.m_txn_id = tx.m_txn_id AND tmpRefund.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpFirstTxn ON tmpFirstTxn.m_txn_id = tx.m_txn_id AND tmpFirstTxn.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpCapture ON tmpCapture.m_txn_id = tx.m_txn_id AND tmpCapture.mongo_m_id = tx.mongo_m_id
                {where} 
                {orderBy}
                {EndResult}";

            return sql;
        }

        [HttpGet]
        public PagedResult<TransactionsEligibleForCancelRow> Get([FromQuery] TransactionsEligibleForCancelArgs args)
        {
            var sql = GetSQL(args);
            var transactions = PageExtensions.FetchPagedResult<TransactionsEligibleForCancelRow>(_db, args.Page, args.PageSize, sql, args);

            return transactions;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery] TransactionsEligibleForCancelArgs args, [FromQuery] OptionArgs optionsArgs)
        {
            var sql = GetSQL(args).ToOptionsSQL(optionsArgs, x => x);   //TRANS-104: Columns names are not mapped anymore, since with de addition of new temp table [tmpFirstTxn1] in GENSPT-409, fields like [tx.mongo_bank_id] aren't valid anymore
            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}
