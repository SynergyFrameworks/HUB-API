using Hub.Transactions.WebAPI.Models;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionByCountrySummary
{
    public class TransactionByCountrySummaryArgs : Args
    {
        public List<string> GlobalIds { get; set; }

        public List<string> BankIds { get; set; }

        public List<string> CorporateIds { get; set; }

        public string MID { get; set; }

        public string Country { get; set; }

        public string PaymentMethod { get; set; }
    }
}
