// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class TransactionBySettlementDateRow
    {
        /// <summary>
        /// Initializes a new instance of the TransactionBySettlementDateRow
        /// class.
        /// </summary>
        public TransactionBySettlementDateRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TransactionBySettlementDateRow
        /// class.
        /// </summary>
        public TransactionBySettlementDateRow(string globalProperty = default(string), string globalId = default(string), string bank = default(string), string bankId = default(string), string corporate = default(string), string corporateId = default(string), string mid = default(string), string merchantId = default(string), System.DateTime? date = default(System.DateTime?), string product = default(string), double? amount = default(double?), string merchantTransactionID = default(string), string responseCode = default(string), string currency = default(string), string transactionType = default(string), string accountHolder = default(string), string bin = default(string), string cryptoId = default(string), string requestorOrigin = default(string), System.DateTime? transactionDate = default(System.DateTime?), string providerReference = default(string), string cardLastFourDigits = default(string), string providerResponseCode = default(string))
        {
            GlobalProperty = globalProperty;
            GlobalId = globalId;
            Bank = bank;
            BankId = bankId;
            Corporate = corporate;
            CorporateId = corporateId;
            Mid = mid;
            MerchantId = merchantId;
            Date = date;
            Product = product;
            Amount = amount;
            MerchantTransactionID = merchantTransactionID;
            ResponseCode = responseCode;
            Currency = currency;
            TransactionType = transactionType;
            AccountHolder = accountHolder;
            Bin = bin;
            CryptoId = cryptoId;
            RequestorOrigin = requestorOrigin;
            TransactionDate = transactionDate;
            ProviderReference = providerReference;
            CardLastFourDigits = cardLastFourDigits;
            ProviderResponseCode = providerResponseCode;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "global")]
        public string GlobalProperty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "globalId")]
        public string GlobalId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bank")]
        public string Bank { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bankId")]
        public string BankId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "corporate")]
        public string Corporate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "corporateId")]
        public string CorporateId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "mid")]
        public string Mid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantId")]
        public string MerchantId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "product")]
        public string Product { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantTransactionID")]
        public string MerchantTransactionID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "responseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "accountHolder")]
        public string AccountHolder { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bin")]
        public string Bin { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cryptoId")]
        public string CryptoId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "requestorOrigin")]
        public string RequestorOrigin { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionDate")]
        public System.DateTime? TransactionDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "providerReference")]
        public string ProviderReference { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cardLastFourDigits")]
        public string CardLastFourDigits { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "providerResponseCode")]
        public string ProviderResponseCode { get; set; }

    }
}
