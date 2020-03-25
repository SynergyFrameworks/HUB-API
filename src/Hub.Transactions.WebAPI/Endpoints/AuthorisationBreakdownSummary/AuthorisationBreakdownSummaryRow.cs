using System;

namespace Hub.Transactions.WebAPI.Endpoints.AuthorisationBreakdownSummary
{
    public class AuthorisationBreakdownSummaryRow
    {
        public string Product { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MID { get; set; }

        public string CardType { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseCodeDescription { get; set; }

        public int Count { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
