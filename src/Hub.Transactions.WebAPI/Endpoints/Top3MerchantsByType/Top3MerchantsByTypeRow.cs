namespace Hub.Transactions.WebAPI.Endpoints.Top3MerchantsByType
{
    public class Top3MerchantsByTypeRow
    {
        public string MID { get; set; }

        public string TransactionType { get; set; }

        public int Count { get; set; }
    }
}
