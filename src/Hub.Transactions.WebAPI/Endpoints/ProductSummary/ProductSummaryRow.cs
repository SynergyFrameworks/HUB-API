namespace Hub.Transactions.WebAPI.Endpoints.ProductSummary
{
    public class ProductSummaryRow
    {
        public string GlobalId { get; set; }

        public string Global { get; set; }

        public string BankId { get; set; }

        public string Bank { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MerchantId { get; set; }

        public string MID { get; set; }

        public string Product { get; set; }

        public string TransactionType { get; set; }

        public int Count { get; set; }

        public string PaymentMethod { get; set; }
    }
}
