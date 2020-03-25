using System;

namespace Hub.Transactions.WebAPI.Endpoints.RiskSummary
{
    public class RiskSummaryRow
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

        public string ResponseCode { get; set; }

        public string ResponseDescription { get; set; }

        public string Product { get; set; }

        public string TransactionType { get; set; }

        public int Count { get; set; }

        public string Status { get; set; }

        public double StatusPercentage { get; set; }
    }
}
