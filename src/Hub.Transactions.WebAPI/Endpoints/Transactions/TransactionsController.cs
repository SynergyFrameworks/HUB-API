using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;

namespace Hub.Transactions.WebAPI.Endpoints.Transactions
{
    [Route("[controller]")]
    public class TransactionsController : Controller
    {
        private readonly IDatabase _db;

        public TransactionsController(IDatabase db)
        {
            _db = db;
        }

        private static string GetSQL(TransactionsArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("((tx.automatic_transaction = true and tx.txn_type != 'PaymentStatus') or (tx.automatic_transaction is null))");
            criteria.AddIfNotNull(args.DateFrom, "tx.req_rcv_at_main_flw >= @DateFrom");
            criteria.AddIfNotNull(args.DateTo, "tx.req_rcv_at_main_flw <= @DateTo");
            criteria.AddIfNotNull(args.GlobalIds, "tx.global_id IN (@GlobalIds)");
            criteria.AddIfNotNull(args.BankIds, "tx.mongo_bank_id IN (@BankIds)");
            criteria.AddIfNotNull(args.CorporateIds, "tx.corp_id IN (@CorporateIds)");
            criteria.AddIfNotNull(args.MerchantIds, "tx.mongo_m_id IN (@MerchantIds)");
            criteria.AddIfNotNull(args.Currencies, "tx.currency IN (@Currencies)");
            criteria.AddIfNotNull(args.AmountFrom, "tx.amount*.001 >= @AmountFrom");
            criteria.AddIfNotNull(args.AmountTo, "tx.amount*.001 <= @AmountTo");
            criteria.AddIfNotNull(args.ResponseCodes, "tx.resp_code IN (@ResponseCodes)");
            criteria.AddIfNotNull(args.BIN, "tx.bin LIKE @BIN", () => args.BIN = string.Format("%{0}%", args.BIN));
            criteria.AddIfNotNull(args.AccountHolder, "tx.acc_holder LIKE @AccountHolder", () => args.AccountHolder = string.Format("%{0}%", args.AccountHolder));
            criteria.AddIfNotNull(args.PaymentMethods, "tx.method_name IN (@PaymentMethods)");
            criteria.AddIfNotNull(args.ExchangeCurrencies, "tx.exch_currency IN (@ExchangeCurrencies)");
            criteria.AddIfNotNull(args.EmailAddress, "tx.customer_email_address LIKE @EmailAddress", () => args.EmailAddress = string.Format("%{0}%", args.EmailAddress));
            criteria.AddIfNotNull(args.PreApprovalStatus, "tx.pre_approval_status = @PreApprovalStatus");
            criteria.AddIfNotNull(args.MerchantTransactionID, "tx.m_txn_id LIKE @MerchantTransactionID", () => args.MerchantTransactionID = string.Format("%{0}%", args.MerchantTransactionID));
            criteria.AddIfNotNull(args.ProviderResponseCodes, "ts.provider_resp_code IN (@ProviderResponseCodes)");
            criteria.AddIfNotNull(args.ProviderTransactionNumber, "ts.provider_transaction_number LIKE @ProviderTransactionNumber", () => args.ProviderTransactionNumber = string.Format("%{0}%", args.ProviderTransactionNumber));
            criteria.AddIfNotNull(args.TransactionTypes, "tx.txn_type IN (@TransactionTypes)");
            criteria.AddIfNotNull(args.APIIDs, "ts.api_id IN (@APIIDs)");
            criteria.AddIfNotNull(args.SettlementDateFrom, "tx.settlement_date >= @SettlementDateFrom");
            criteria.AddIfNotNull(args.SettlementDateTo, "tx.settlement_date <= @SettlementDateTo");
            criteria.AddIfNotNull(args.TransactionSources, "tx.requestor_origin IN (@TransactionSources)");
            criteria.AddIfNotNull(args.CardLastFourDigits, "tx.card_last_four_digits = @CardLastFourDigits");
            criteria.AddIfNotNull(args.CardTypes, "tx.card_type IN (@CardTypes)");
            criteria.AddIfNotNull(args.UserID, "tx.userid LIKE @UserID", () => args.UserID = string.Format("%{0}%", args.UserID));
            criteria.AddIfNotNull(args.UIUserID, "tx.ui_user_id LIKE @UIUserID", () => args.UIUserID = string.Format("%{0}%", args.UIUserID));
            criteria.AddIfNotNull(args.IPAddress, "tx.customer_ip_address LIKE @IPAddress", () => args.IPAddress = string.Format("%{0}%", args.IPAddress));
            criteria.AddIfNotNull(args.BankMerchantID, "tx.bank_merchant_id LIKE @BankMerchantID", () => args.BankMerchantID = string.Format("%{0}%", args.BankMerchantID));
            criteria.AddIfNotNull(args.Token, "tx.token LIKE @Token", () => args.Token = string.Format("%{0}%", args.Token));
            criteria.AddIfNotNull(args.TerminalID, "tx.terminal_id LIKE @TerminalID", () => args.TerminalID = string.Format("%{0}%", args.TerminalID));
            criteria.AddIfNotNull(args.PaymentID, "tx.apc_txn_id LIKE @PaymentID", () => args.PaymentID = string.Format("%{0}%", args.PaymentID));
            criteria.AddIfNotNull(args.BankAuthID, "tx.bank_auth_id LIKE @BankAuthID", () => args.BankAuthID = string.Format("%{0}%", args.BankAuthID));
            criteria.AddIfNotNull(args.Acquirer, "tx.acquirer_name LIKE @Acquirer", () => args.Acquirer = string.Format("%{0}%", args.Acquirer));
            criteria.AddIfNotNull(args.OriginalSettlementDateFrom, "tx.original_settlement_date >= @OriginalSettlementDateFrom");
            criteria.AddIfNotNull(args.OriginalSettlementDateTo, "tx.original_settlement_date <= @OriginalSettlementDateTo");
            var where = criteria.ToWhereClause();
            var orderBy = args.OrderBys.ToOrderByClause("ID DESC");

            var sql = $@"SELECT 
                    ts.id AS ID, 
                    tx.cs_rpt_txn_id AS TransactionID, 
                    tx.global_id AS GlobalId, 
                    tx.global_name AS Global, 
                    tx.bank_id AS Bank_ID, 
                    tx.bank_name AS Bank, 
                    tx.corp_id AS CorporateId, 
                    tx.corp_name AS Corporate, 
                    tx.mid AS MID, 
                    tx.merchant_name AS Merchant, 
                    tx.acquirer_name AS Acquirer, 
                    tx.m_txn_id AS MerchantTransactionID, 
                    ts.provider_transaction_number AS ProviderTransactionNumber, 
                    tx.txn_type AS TransactionType, 
                    ts.api_id AS APIId,
                    ts.api_name AS Product, 
                    tx.resp_code AS ResponseCode, 
                    tx.resp_code_desc AS ResponseCodeDescription, 
                    COALESCE(NULLIF(tx.method_name,''), '[unknown]') AS PaymentMethod, 
                    tx.currency AS Currency, 
                    tx.amount * .001 AS Amount, 
                    tx.customer_email_address AS CustomerEmailAddress, 
                    tx.bin AS BIN, 
                    tx.acc_holder AS AccountHolder, 
                    tx.mongo_bank_id AS BankId, 
                    tx.mongo_m_id AS MerchantId, 
                    tx.req_rcv_at_main_flw + (@TimeZoneOffset ||' minutes')::interval AS Date, 
                    tx.crypto_key AS CryptoId, 
                    case when tx.exch_rate > '' then cast(tx.exch_rate as numeric) else 0 end AS ExchangeRate, 
                    case when tx.exch_amount > '' then cast(tx.exch_amount as numeric) * .001 else 0 end AS ExchangeAmount, 
                    tx.exch_currency AS ExchangeCurrency,
                    tx.settlement_date::timestamp without time zone AS SettlementDate,
                    tx.requestor_origin AS TransactionSource,
                    case
                        when tx.requestor_origin = 'API' then 'API' 
                        when tx.requestor_origin = 'VT' then 'Virtual Terminal' 
                        when tx.requestor_origin = 'TM' then 'Transaction Management' 
                        when tx.requestor_origin = 'HPP' then 'Hosted Pay Page'
                    end AS TransactionSourceName,
                    tx.card_last_four_digits AS CardLastFourDigits,
                    tx.card_type AS CardType,
                    tx.userid AS UserID,
                    tx.ui_user_id AS UIUserID,
                    tx.customer_ip_address AS IPAddress,
                    tx.token AS Token,
                    tx.terminal_id AS TerminalID,
                    tx.bank_merchant_id AS BankMerchantID,
                    tx.bank_auth_id AS BankAuthID,
                    tx.apc_txn_id AS PaymentID,
                    ts.provider_resp_code || ' ' || ts.provider_resp_code_desc AS ProviderResponse,
                    ts.provider_resp_code AS ProviderResponseCode,
                    ts.provider_resp_code_desc AS ProviderResponseCodeDescription,
                    tx.exp_month AS ExpiraryMonth,
                    tx.exp_year AS ExpiraryYear,
                    tx.original_settlement_date::timestamp without time zone AS OriginalSettlementDate,
                    case 
                        when tx.unmatched_transaction = 'true' then 'Y'
                    end AS UnmatchedTransaction
                 FROM cs_rpt_txn tx 
                    LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                {where}
                {orderBy}";

            return sql;
        }


        [HttpGet]
        public PagedResult<TransactionsRow> Get([FromQuery] TransactionsArgs args)
        {
            var sql = GetSQL(args);
            var transactions = PageExtensions.FetchPagedResult<TransactionsRow>(_db, args.Page, args.PageSize, sql, args);

            return transactions;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]TransactionsArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            var sql = GetSQL(args).ToOptionsSQL(optionsArgs);
            var result = _db.Fetch<FilterOption>(sql, args);

            return result;
        }
    }
}