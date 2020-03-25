using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionBySettlementSummary
{
    public class TransactionBySettlementSummaryRow
    {
        public string Global { get; set; }

        public string GlobalId { get; set; }

        public string Bank { get; set; }

        public string BankId { get; set; }

        public string Corporate { get; set; }

        public string CorporateId { get; set; }

        public string MID { get; set; }

        public string MerchantId { get; set; }

        public DateTime Date { get; set; }

        public double DebitTotal { get; set; }

        public double CreditTotal { get; set; }

        public string Currency { get; set; }

    }
}
