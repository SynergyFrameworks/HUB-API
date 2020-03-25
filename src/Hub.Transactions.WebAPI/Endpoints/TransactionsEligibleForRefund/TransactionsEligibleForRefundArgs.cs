using Hub.Transactions.WebAPI.Models;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionsEligibleForRefund
{
    public class TransactionsEligibleForRefundArgs : Args
    {
        public List<string> GlobalIds { get; set; }

        public List<string> BankIds { get; set; }

        public List<string> CorporateIds { get; set; }

        public List<string> MerchantIds { get; set; }

        public string MerchantTransactionID { get; set; }

        public List<string> Currencies { get; set; }

        public List<string> ExchangeCurrencies { get; set; }

        public List<string> APIIds { get; set; }

        public string BIN { get; set; }

        public List<string> TransactionTypes { get; set; }

        public List<string> ResponseCodes { get; set; }

        public string LoginUser { get; set; }
    }
}
