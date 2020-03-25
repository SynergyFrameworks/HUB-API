namespace Hub.Transactions.WebAPI.Endpoints.PaymentMethodBreakdown
{
    public class PaymentMethodBreakdownRow
    {
        public string PaymentMethod { get; set; }

        public string Currency { get; set; }

        public string Global { get; set; }

        public string GlobalId { get; set; }

        public string Bank { get; set; }

        // should be BankID, but that name conflicts with BankId with PetaPoco 
        public string Bank_ID { get; set; }

        public string Corporate { get; set; }

        public string CorporateId { get; set; }

        public string Merchant { get; set; }

        public string MerchantID { get; set; }

        public string MID { get; set; }

        public int CaptureCount { get; set; }

        public double CaptureCountPercentage { get; set; }

        public double CaptureValue { get; set; }

        public double CaptureValuePercentage { get; set; }

        public double AverageValue { get; set; }

        public string BINCountry { get; set; }
    }
}
