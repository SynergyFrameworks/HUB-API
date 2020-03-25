namespace Hub.Transactions.WebAPI.Endpoints.CardTypeBreakdown
{
    public class CardTypeBreakdownRow
    {
        public string Product { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public string BINCountry { get; set; }

        public string CardType { get; set; }

        public int CapturePaymentCount { get; set; }
    }
}
