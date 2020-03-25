// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class TransactionsEligibleForRefundRow
    {
        /// <summary>
        /// Initializes a new instance of the TransactionsEligibleForRefundRow
        /// class.
        /// </summary>
        public TransactionsEligibleForRefundRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TransactionsEligibleForRefundRow
        /// class.
        /// </summary>
        public TransactionsEligibleForRefundRow(string id = default(string), string globalId = default(string), string globalProperty = default(string), string bankId = default(string), string bank = default(string), string corporateId = default(string), string corporate = default(string), string mid = default(string), string merchantId = default(string), string merchant = default(string), System.DateTime? date = default(System.DateTime?), string merchantTransactionID = default(string), string product = default(string), string apiId = default(string), double? amount = default(double?), double? amountEligibleForRefund = default(double?), string currency = default(string), string accountHolder = default(string), string bin = default(string), string cryptoId = default(string), double? exchangeRate = default(double?), double? exchangeAmount = default(double?), string exchangeCurrency = default(string), string transactionType = default(string), string responseCode = default(string), string responseCodeDescription = default(string))
        {
            Id = id;
            GlobalId = globalId;
            GlobalProperty = globalProperty;
            BankId = bankId;
            Bank = bank;
            CorporateId = corporateId;
            Corporate = corporate;
            Mid = mid;
            MerchantId = merchantId;
            Merchant = merchant;
            Date = date;
            MerchantTransactionID = merchantTransactionID;
            Product = product;
            ApiId = apiId;
            Amount = amount;
            AmountEligibleForRefund = amountEligibleForRefund;
            Currency = currency;
            AccountHolder = accountHolder;
            Bin = bin;
            CryptoId = cryptoId;
            ExchangeRate = exchangeRate;
            ExchangeAmount = exchangeAmount;
            ExchangeCurrency = exchangeCurrency;
            TransactionType = transactionType;
            ResponseCode = responseCode;
            ResponseCodeDescription = responseCodeDescription;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "globalId")]
        public string GlobalId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "global")]
        public string GlobalProperty { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bankId")]
        public string BankId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bank")]
        public string Bank { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "corporateId")]
        public string CorporateId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "corporate")]
        public string Corporate { get; set; }

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
        [JsonProperty(PropertyName = "merchant")]
        public string Merchant { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "merchantTransactionID")]
        public string MerchantTransactionID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "product")]
        public string Product { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "apiId")]
        public string ApiId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "amountEligibleForRefund")]
        public double? AmountEligibleForRefund { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

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
        [JsonProperty(PropertyName = "exchangeRate")]
        public double? ExchangeRate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "exchangeAmount")]
        public double? ExchangeAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "exchangeCurrency")]
        public string ExchangeCurrency { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "responseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "responseCodeDescription")]
        public string ResponseCodeDescription { get; set; }

    }
}
