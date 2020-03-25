using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionManagementResults
{
    public class TransactionManagementResultsRow
    {
        public int ID { get; set; }

        public int TransactionID { get; set; }

        public string APCTransactionID { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryYear { get; set; }

        public string CardHolder { get; set; }

        public string CardNumber { get; set; }

        public string GlobalId { get; set; }

        public string BankID { get; set; }

        public string MID { get; set; }

        public string Product { get; set; }

        public string APIId { get; set; }

        public string TransactionType { get; set; }

        public string UserID { get; set; }

        public string RequestorID { get; set; }

        public Int64 Amount { get; set; }

        public string ChannelType { get; set; }

        public string Currency { get; set; }

        public string MerchantTxnID { get; set; }

        public string PaymentMethod { get; set; }

        public string CryptoId { get; set; }
    }
}
