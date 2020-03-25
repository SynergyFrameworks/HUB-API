using System;
namespace Hub.Transactions.WebAPI.Endpoints.RFIDetails
{
    public class RFIDetailsRow
    {
        public string RecordType { get; set; }
        public DateTime? RFIRequestedDate { get; set; }
        public DateTime? DeadlineDate { get; set; }
        public string Addressee { get; set; }
        public string CaseNumber { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiry { get; set; }
        public string AcRef { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? TransactionTime { get; set; }
        public string TransactionAmount { get; set; }
        public string Currency { get; set; }
        public string TypeOfTransaction { get; set; }
        public string MerchantName { get; set; }
        public string MerchantLocation { get; set; }
        public string MerchantCountry { get; set; }
        public string MerchantStoreNumber { get; set; }
        public string FacilityNrAndTerminal { get; set; }
        public string AuthCode { get; set; }
        public string RRN { get; set; }
}
}
