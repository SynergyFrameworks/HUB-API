using System;

namespace Hub.Transactions.WebAPI.Endpoints.Transactions
{
    public class TransactionsRow
    {
        public DateTime Date { get; set; }

        public string ID { get; set; }

        public string TransactionID { get; set; }

        public string GlobalId { get; set; }

        public string Global { get; set; }

        // should be BankID, but that name conflicts with BankId with PetaPoco 
        public string Bank_ID { get; set; }

        public string Bank { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MID { get; set; }

        public string Merchant { get; set; }

        public string MerchantTransactionID { get; set; }
        public string ProviderTransactionNumber { get; set; }

        public string TransactionType { get; set; }

        public string Product { get; set; }

        public string APIId { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseCodeDescription { get; set; }

        public string PaymentMethod { get; set; }

        public double Amount { get; set; }

        public string Currency { get; set; }

        public string CustomerEmailAddress { get; set; }

        public string AccountHolder { get; set; }

        public string MerchantId { get; set; }

        public string BankId { get; set; }

        public string BIN { get; set; }

        public string CryptoId { get; set; }

        public double ExchangeRate { get; set; }

        public double ExchangeAmount { get; set; }

        public string ExchangeCurrency { get; set; }

        public DateTime? SettlementDate { get; set; }

        public string TransactionSource { get; set; }

        public string TransactionSourceName { get; set; }

        public string CardLastFourDigits { get; set; }

        public string CardType { get; set; }

        public string UserID { get; set; }

        public string UIUserID { get; set; }

        public string BankMerchantID { get; set; }

        public string Token { get; set; }

        public string IPAddress { get; set; }

        public string TerminalID { get; set; }

        public string PaymentID { get; set; }

        public string BankAuthID { get; set; }

        public string ProviderResponse { get; set; }

        public string ExpiraryMonth { get; set; }

        public string ExpiraryYear { get; set; }

        public string Acquirer { get; set; }

        public string ProviderResponseCode { get; set; }

        public string ProviderResponseCodeDescription { get; set; }

        public DateTime? OriginalSettlementDate { get; set; }

        public string UnmatchedTransaction { get; set; }
    }
}