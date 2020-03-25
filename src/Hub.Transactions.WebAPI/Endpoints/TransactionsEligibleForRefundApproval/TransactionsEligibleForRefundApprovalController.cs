using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionsEligibleForRefundApproval
{
    [Route("[controller]")]
    public class TransactionsEligibleForRefundApprovalController
    {
        private readonly IDatabase _db;

        public TransactionsEligibleForRefundApprovalController(IDatabase db)
        {
            _db = db;
        }

        private static string GetSQL(TransactionsEligibleForRefundApprovalArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.resp_code = '1000'");
            criteria.Add("tx.txn_type = 'PreApproveRefund'");
            criteria.Add("tx.pre_approval_status = 'PENDING'");
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
            //criteria.AddIfNotNull(args.APIIds, "ts.api_id IN (@APIIds)");
            criteria.AddIfNotNull(args.ResponseCodes, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.TransactionTypes, "tx.txn_type IN (@TransactionTypes)");
            criteria.AddIfNotNull(args.LoginUser, "((tx.ui_user_id is null) or (tx.ui_user_id != @LoginUser))");
            criteria.Add("((select  count (cs_rpt_txn_id) from cs_rpt_txn where m_txn_id = tx.m_txn_id AND  (txn_type = 'OneStepPayment' OR txn_type = 'RefundPayment'))   < 2 )");
            var where = criteria.ToWhereClause();
            var orderBy = args.OrderBys.ToOrderByClause("ID DESC, Date");

            var sql = $@"SELECT 
                   case when ts.id isnull then  (select id from cs_rpt_timestamp where  resp_code = '1000' AND cs_rpt_txn_id = (select  cs_rpt_txn_id from cs_rpt_txn where m_txn_id=tx.m_txn_id order by cs_rpt_txn_id  asc limit 1) order by id desc limit 1) else ts.id end AS ID, 
                    tx.global_id AS GlobalId, 
                    tx.global_name AS Global, 
                    tx.mongo_bank_id AS BankId, 
                    tx.bank_name AS Bank,
                    tx.corp_id AS CorporateId, 
                    tx.corp_name AS Corporate, 
                    tx.mongo_m_id AS MerchantId,
                    tx.merchant_name AS Merchant,
                    tx.mid AS MID, 
case when ts.api_id isnull then  (select api_id from cs_rpt_timestamp where resp_code = '1000' AND cs_rpt_txn_id = (select  cs_rpt_txn_id from cs_rpt_txn where m_txn_id=tx.m_txn_id order by cs_rpt_txn_id  asc limit 1) order by id desc limit 1) else ts.api_id end AS APIId, 
case when ts.api_name isnull then  (select api_name from cs_rpt_timestamp where resp_code = '1000' AND cs_rpt_txn_id = (select  cs_rpt_txn_id from cs_rpt_txn where m_txn_id=tx.m_txn_id order by cs_rpt_txn_id  asc limit 1) order by id desc limit 1) else ts.api_name end AS Product,  
                    tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval AS Date,
                    tx.m_txn_id AS MerchantTransactionID, 
                    tx.amount*.001 AS Amount, 
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
                    LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id AND ts.api_id IN (@APIIds)
                {where}
                {orderBy}";

            return sql;
        }

        [HttpGet]
        public PagedResult<TransactionsEligibleForRefundApprovalRow> Get([FromQuery] TransactionsEligibleForRefundApprovalArgs args)
        {
            var sql = GetSQL(args);
            var transactions = PageExtensions.FetchPagedResult<TransactionsEligibleForRefundApprovalRow>(_db, args.Page, args.PageSize, sql, args);

            return transactions;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery] TransactionsEligibleForRefundApprovalArgs args, [FromQuery] OptionArgs optionsArgs)
        {            
            var sql = GetSQL(args);

            var valueColumn = MapToColumn(optionsArgs.OptionValue);
            var textColumn = MapToColumn(optionsArgs.OptionText);

            var binding = $@"WITH tmp AS({sql})";

            var optionsSQL = $@"{binding}
                            SELECT DISTINCT
                                {valueColumn} AS Key,
                                {textColumn} AS Value 
                            from cs_rpt_txn tx LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id AND ts.api_id IN (@APIIds) LEFT JOIN tmp ON tx.m_txn_id = tmp.MerchantTransactionID AND tmp.MerchantId = tx.mongo_m_id where tx.m_txn_id = tmp.MerchantTransactionID AND (tx.txn_type = 'OneStepPayment' OR tx.txn_type = 'RefundPayment')
                                AND {valueColumn} <> '' 
                                AND {textColumn} <> ''
                            ORDER BY {textColumn}";

            var result = _db.Fetch<FilterOption>(optionsSQL, args);

            return result;
        }

        private static string MapToColumn(string propertyName)
        {           
            switch (propertyName)
            {
                case "Global": return "tx.global_name";
                case "GlobalId": return "tx.global_id";
                case "Bank": return "tx.bank_name";
                case "BankId": return "tx.mongo_bank_id";
                case "CorporateId": return "tx.corp_id";
                case "Corporate": return "tx.corp_name";
                case "MerchantId": return "tx.mongo_m_id";
                case "Merchant": return "tx.merchant_name";
                case "MID": return "tx.mid";
                case "Product": return "ts.api_name";
                case "APIId": return "ts.api_id";
                case "CardType": return "tx.card_type";
                case "ResponseCode": return "tx.resp_code";
                case "ResponseCodeDescription": return "ts.resp_code_desc";
                case "BIN": return "tx.Bin";
                case "BINCountry": return "tx.bin_country";
                case "PaymentMethod": return "tx.method_name";
                case "TransactionType": return "tx.txn_type";
                case "Currency": return "tx.currency";
                case "ExchangeCurrency": return "tx.exch_currency";
                case "Country": return "tx.billing_address_country";
                case "AccountHolder": return "tx.acc_holder";
                case "ProviderResponseCode": return "ts.provider_resp_code";
                case "TransactionSource": return "tx.requestor_origin";

                default: return null;
            }
        }
    }
}
