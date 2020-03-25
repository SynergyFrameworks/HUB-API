using System;

namespace Hub.Transactions.WebAPI.Endpoints.AuthorisationSummary
{
    public class AuthorisationSummaryRow
    {
        public string Product { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MID { get; set; }

        public DateTime Date { get; set; }

        public string CardType { get; set; }

        public int AuthCount { get; set; }

        public int DeclineCount { get; set; }
    }
}
