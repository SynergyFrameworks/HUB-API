namespace Hub.Transactions.WebAPI.Endpoints.RefundRates
{
    public class RefundRatesRow
    {
        public string Product { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string MID { get; set; }

        public string Currency { get; set; }

        public string CardType { get; set; }

        public double SettlementAmount { get; set; }

        public int SettlementCount { get; set; }

        public double RefundAmount { get; set; }

        public int RefundCount { get; set; }

        public double ExchangeRate { get; set; }

        public double ExchangeAmount { get; set; }

        public string ExchangeCurrency { get; set; }
    }
}
