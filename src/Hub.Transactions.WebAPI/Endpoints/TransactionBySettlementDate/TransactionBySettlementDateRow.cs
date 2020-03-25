using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionBySettlementDate
{
    public class TransactionBySettlementDateRow
    {
        public string Global { get; set; }

        public string GlobalId { get; set; }

        public string Bank { get; set; }

        public string BankId { get; set; }

        public string Corporate { get; set; }

        public string CorporateId { get; set; }

        public string MID { get; set; }

        public string MerchantId { get; set; }

        public DateTime Date { get; set; }

        public string Product { get; set; }

        public double Amount { get; set; }

        public string MerchantTransactionID { get; set; }

        public string ResponseCode { get; set; }

        public string Currency { get; set; }

        public string TransactionType { get; set; }

        public string AccountHolder { get; set; }

        public string BIN { get; set; }

        public string CryptoId { get; set; }

        public string RequestorOrigin { get; set; }

        public DateTime TransactionDate { get; set; }

        public string ProviderReference { get; set; }

        public string CardLastFourDigits { get; set; }

        public string ProviderResponseCode { get; set; }
    }
}
