using System;

namespace Hub.Transactions.WebAPI.Endpoints.CaptureRefundSummary
{
    public class CaptureRefundSummaryRow
    {
        public string Product { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string BankId { get; set; }

        public string Bank { get; set; }

        public string BINCountry { get; set; }

        public string MID { get; set; }

        public DateTime Date { get; set; }

        public string CardType { get; set; }

        public string TransactionType { get; set; }

        public string Currency { get; set; }

        public int Count { get; set; }

        public double TotalAmount { get; set; }

        public string PaymentMethod { get; set; }

        public double ExchangeRate { get; set; }

        public double ExchangeAmount { get; set; }

        public string ExchangeCurrency { get; set; }
    }
}
