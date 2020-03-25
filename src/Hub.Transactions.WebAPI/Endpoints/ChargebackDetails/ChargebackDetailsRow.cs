using System;
namespace Hub.Transactions.WebAPI.Endpoints.ChargebackDetails
{
    public class ChargebackDetailsRow
    {
        public string RecordType { get; set; }
        public DateTime? Date { get; set; }
        public string Addressee { get; set; }
        public string CaseNumber { get; set; }
        public string CardNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string SettlementAmount { get; set; }
        public string SettlementCurrency { get; set; }
        public string TypeOfTransaction { get; set; }
        public string MerchantName { get; set; }
        public string MerchantLocation { get; set; }
        public string MerchantCountry { get; set; }
        public string MerchantStoreNumber { get; set; }
        public string AuthCode { get; set; }
        public string Comment { get; set; }
        public string AdditionalInfo { get; set; }
        public string RRN { get; set; }
        public string TerminalID { get; set; }
        public string MerchantID { get; set; }
        public string RelatedRefs { get; set; }
        public string AcquirerRef { get; set; }
    }
}
