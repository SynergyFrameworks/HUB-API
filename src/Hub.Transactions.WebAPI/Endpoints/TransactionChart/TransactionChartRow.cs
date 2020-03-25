namespace Hub.Transactions.WebAPI.Endpoints.TransactionChart
{
    public class TransactionChartRow
    {
        public string TransactionType { get; set; }

        public int DayNo { get; set; }

        public string Day { get; set; }

        public int Count { get; set; }
    }
}
