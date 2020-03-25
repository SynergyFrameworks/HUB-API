using System;

namespace Hub.Transactions.WebAPI.Endpoints.AuthResponseReason
{
    public class AuthResponseReasonRow
    {
        public DateTime TransactionDate { get; set; }

        public string Global { get; set; }

        public string GlobalId { get; set; }

        public string Bank { get; set; }

        public string BankId { get; set; }

        public string Corporate { get; set; }

        public string CorporateId { get; set; }

        public string MID { get; set; }

        public string Merchant { get; set; }

        public string MerchantId { get; set; }

        public string CardType { get; set; }

        public string BIN { get; set; }

        public string ResponseCode { get; set; }

        public string ResponseDescription { get; set; }

        public int Count { get; set; }

        public double ResponseCodesPercentage { get; set; }

    }
}
