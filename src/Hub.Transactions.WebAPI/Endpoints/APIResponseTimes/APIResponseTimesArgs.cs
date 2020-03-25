using Hub.Transactions.WebAPI.Models;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.APIResponseTimes
{
    public class APIResponseTimesArgs : Args
    {
        public List<string> GlobalIds { get; set; }

        public List<string> BankIds { get; set; }

        public List<string> CorporateIds { get; set; }

        public List<string> MerchantIds { get; set; }

        public bool GroupByBankId { get; set; }
    }
}
