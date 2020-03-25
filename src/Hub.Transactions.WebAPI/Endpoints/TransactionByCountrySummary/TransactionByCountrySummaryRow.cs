using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionByCountrySummary
{
    public class TransactionByCountrySummaryRow
    {
        public string Product { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MID { get; set; }

        public string Country { get; set; }

        public DateTime Date { get; set; }

        public string PaymentMethod { get; set; }

        public string CardType { get; set; }

        public int SuccessCount { get; set; }

        public int FailCount { get; set; }
    }
}
