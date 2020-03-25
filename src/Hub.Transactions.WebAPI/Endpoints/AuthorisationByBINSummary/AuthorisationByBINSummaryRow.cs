using System;

namespace Hub.Transactions.WebAPI.Endpoints.AuthorisationByBINSummary
{
    public class AuthorisationByBINSummaryRow
    {
        public string Product { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public DateTime Date { get; set; }

        public string BIN { get; set; }

        public string BINCountry { get; set; }

        public int AuthCount { get; set; }

        public int DeclineCount { get; set; }
    }
}
