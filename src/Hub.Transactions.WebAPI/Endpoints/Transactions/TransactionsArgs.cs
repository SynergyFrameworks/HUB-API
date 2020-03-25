using Hub.Transactions.WebAPI.Models;
using System;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.Transactions
{
    public class TransactionsArgs : Args
    {
        private double? _amountFrom;
        private double? _amountTo;
        public List<string> TransactionTypes { get; set; }

        public List<string> Currencies { get; set; }

        public double? AmountFrom
        {
            get
            {
                if (_amountFrom.HasValue == false || _amountTo.HasValue == false)
                {
                    return _amountFrom;
                }

                var amount = Math.Min(_amountFrom.Value, _amountTo.Value);

                return amount;
            }
            set { _amountFrom = value; }
        }

        public double? AmountTo
        {
            get
            {
                if (_amountFrom.HasValue == false || _amountTo.HasValue == false)
                {
                    return _amountTo;
                }

                var amount = Math.Max(_amountFrom.Value, _amountTo.Value);

                return amount;
            }
            set { _amountTo = value; }
        }

        public string AccountHolder { get; set; }

        public List<string> ResponseCodes { get; set; }

        public string BIN { get; set; }

        public List<string> GlobalIds { get; set; }

        public List<string> BankIds { get; set; }

        public List<string> CorporateIds { get; set; }

        public List<string> MerchantIds { get; set; }

        public List<string> APIIDs { get; set; }

        public string MerchantTransactionID { get; set; }

        public string ProviderTransactionNumber { get; set; }

        public List<string> ProviderResponseCodes { get; set; }

        public string PreApprovalStatus { get; set; }

        public List<string> ExchangeCurrencies { get; set; }

        public string EmailAddress { get; set; }

        public List<string> PaymentMethods { get; set; }

        public DateTime? SettlementDateFrom { get; set; }

        public DateTime? SettlementDateTo { get; set; }

        public List<string> TransactionSources { get; set; }

        public string CardLastFourDigits { get; set; }

        public List<string> CardTypes { get; set; }

        public string UserID { get; set; }

        public string UIUserID { get; set; }

        public string BankMerchantID { get; set; }

        public string Token { get; set; }

        public string IPAddress { get; set; }

        public string TerminalID { get; set; }

        public string PaymentID { get; set; }

        public string BankAuthID { get; set; }

        public string Acquirer { get; set; }

        public DateTime? OriginalSettlementDateFrom { get; set; }

        public DateTime? OriginalSettlementDateTo { get; set; }
    }
}