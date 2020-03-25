using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionSummary
{
    public class TransactionSummaryRow
    {
        public string APIId { get; set; }

        public string Product { get; set; }

        public string Currency { get; set; }

        public double AverageAmount { get; set; }

        public double SuccessRate { get; set; }

        public int TotalVolume { get; set; }

        public double AverageSeconds { get; set; }

        public int TotalCountry { get; set; }

        public DateTime LastDate { get; set; }
    }
}
