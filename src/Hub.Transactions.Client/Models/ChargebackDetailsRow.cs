// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class ChargebackDetailsRow
    {
        /// <summary>
        /// Initializes a new instance of the ChargebackDetailsRow class.
        /// </summary>
        public ChargebackDetailsRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ChargebackDetailsRow class.
        /// </summary>
        public ChargebackDetailsRow(string recordType = default(string), System.DateTime? date = default(System.DateTime?), string addressee = default(string), string caseNumber = default(string), string cardNumber = default(string), System.DateTime? transactionDate = default(System.DateTime?), string transactionAmount = default(string), string currencyCode = default(string), string settlementAmount = default(string), string settlementCurrency = default(string), string typeOfTransaction = default(string), string merchantName = default(string), string merchantLocation = default(string), string merchantCountry = default(string), string merchantStoreNumber = default(string), string authCode = default(string), string comment = default(string), string additionalInfo = default(string), string rrn = default(string), string terminalID = default(string), string merchantID = default(string), string relatedRefs = default(string), string acquirerRef = default(string))
        {
            RecordType = recordType;
            Date = date;
            Addressee = addressee;
            CaseNumber = caseNumber;
            CardNumber = cardNumber;
            TransactionDate = transactionDate;
            TransactionAmount = transactionAmount;
            CurrencyCode = currencyCode;
            SettlementAmount = settlementAmount;
            SettlementCurrency = settlementCurrency;
            TypeOfTransaction = typeOfTransaction;
            MerchantName = merchantName;
            MerchantLocation = merchantLocation;
            MerchantCountry = merchantCountry;
            MerchantStoreNumber = merchantStoreNumber;
            AuthCode = authCode;
            Comment = comment;
            AdditionalInfo = additionalInfo;
            Rrn = rrn;
            TerminalID = terminalID;
            MerchantID = merchantID;
            RelatedRefs = relatedRefs;
            AcquirerRef = acquirerRef;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "recordType")]
        public string RecordType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "addressee")]
        public string Addressee { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "caseNumber")]
        public string CaseNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cardNumber")]
        public string CardNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionDate")]
        public System.DateTime? TransactionDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionAmount")]
        public string TransactionAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currencyCode")]
        public string CurrencyCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "settlementAmount")]
        public string SettlementAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "settlementCurrency")]
        public string SettlementCurrency { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "typeOfTransaction")]
        public string TypeOfTransaction { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantName")]
        public string MerchantName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantLocation")]
        public string MerchantLocation { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantCountry")]
        public string MerchantCountry { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantStoreNumber")]
        public string MerchantStoreNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "authCode")]
        public string AuthCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "comment")]
        public string Comment { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "additionalInfo")]
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "rrn")]
        public string Rrn { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "terminalID")]
        public string TerminalID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantID")]
        public string MerchantID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "relatedRefs")]
        public string RelatedRefs { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "acquirerRef")]
        public string AcquirerRef { get; set; }

    }
}
