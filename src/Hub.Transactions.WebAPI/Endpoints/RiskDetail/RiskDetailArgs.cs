using Hub.Transactions.WebAPI.Models;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.RiskDetail
{
    public class RiskDetailArgs : Args
    {
        public List<string> TransactionTypes { get; set; }

        public List<string> Currencies { get; set; }

        public List<string> ResponseCodes { get; set; }

        public string BIN { get; set; }

        public List<string> GlobalIds { get; set; }

        public List<string> BankIds { get; set; }

        public List<string> CorporateIds { get; set; }

        public List<string> MerchantIds { get; set; }

        public string MerchantTransactionID { get; set; }

        public string Product { get; set; }

        public List<string> ExchangeCurrencies { get; set; }

        public string EmailAddress { get; set; }

        public List<string> PaymentMethods { get; set; }
    }
}
