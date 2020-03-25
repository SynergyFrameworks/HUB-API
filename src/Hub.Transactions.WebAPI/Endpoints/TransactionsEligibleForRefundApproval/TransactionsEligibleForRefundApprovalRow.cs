using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionsEligibleForRefundApproval
{
    public class TransactionsEligibleForRefundApprovalRow
    {
        public string ID { get; set; }

        public string GlobalId { get; set; }

        public string Global { get; set; }

        public string BankId { get; set; }

        public string Bank { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MID { get; set; }

        public string MerchantId { get; set; }

        public string Merchant { get; set; }

        public DateTime Date { get; set; }

        public string MerchantTransactionID { get; set; }

        public string Product { get; set; }

        public string APIId { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public string AccountHolder { get; set; }

        public string BIN { get; set; }

        public string CryptoId { get; set; }

        public double ExchangeRate { get; set; }

        public double ExchangeAmount { get; set; }

        public string ExchangeCurrency { get; set; }

        public string TransactionType { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseCodeDescription { get; set; }
    }
}
