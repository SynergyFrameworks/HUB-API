namespace Hub.Transactions.WebAPI.Endpoints.APIResponseTimes
{
    public class APIResponseTimesRow
    {
        public double TotalDurationTransaction { get; set; }

        public double TotalDurationProviders { get; set; }

        public double TotalDurationAPC { get; set; }

        public string Corporate { get; set; }

        public string CorporateId { get; set; }

        public string MID { get; set; }

        public string MerchantId { get; set; }

        public string BankId { get; set; }

        public string Bank { get; set; }

        public string GlobalId { get; set; }

        public string Global { get; set; }

        public int NumberOfRequests { get; set; }
    }
}
