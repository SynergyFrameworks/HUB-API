using System;

namespace Hub.Transactions.WebAPI.Endpoints.RiskDetail
{
    public class RiskDetailRow
    {
        public string ID { get; set; }

        public DateTime Date { get; set; }

        public string GlobalId { get; set; }

        public string Global { get; set; }

        public string BankId { get; set; }

        public string Bank { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MID { get; set; }

        public string MerchantId { get; set; }

        public string Merchant { get; set; }

        public string MerchantTransactionID { get; set; }

        public string TransactionType { get; set; }

        public string Product { get; set; }

        public string ProductId { get; set; }

        public string PaymentMethod { get; set; }

        public string Currency { get; set; }

        public double Amount { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmailAddress { get; set; }

        public string CustomerIPAddress { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseCodeDescription { get; set; }

        public string RiskScore { get; set; }

        public string RiskStatus { get; set; }

        public double ExchangeRate { get; set; }

        public double ExchangeAmount { get; set; }

        public string ExchangeCurrency { get; set; }
    }
}
