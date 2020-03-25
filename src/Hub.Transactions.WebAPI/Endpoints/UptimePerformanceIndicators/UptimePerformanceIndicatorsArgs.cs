using Hub.Transactions.WebAPI.Models;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.UptimePerformanceIndicators
{
    public class UptimePerformanceIndicatorsArgs : Args
    {
        public List<string> GlobalIds { get; set; }

        public List<string> BankIds { get; set; }

        public List<string> CorporateIds { get; set; }

        public List<string> MerchantIds { get; set; }

        public string MID { get; set; }

        public bool? GroupByBankId { get; set; }
    }
}
