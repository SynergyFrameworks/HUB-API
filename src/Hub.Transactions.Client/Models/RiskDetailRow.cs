// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class RiskDetailRow
    {
        /// <summary>
        /// Initializes a new instance of the RiskDetailRow class.
        /// </summary>
        public RiskDetailRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RiskDetailRow class.
        /// </summary>
        public RiskDetailRow(string id = default(string), System.DateTime? date = default(System.DateTime?), string globalId = default(string), string globalProperty = default(string), string bankId = default(string), string bank = default(string), string corporateId = default(string), string corporate = default(string), string mid = default(string), string merchantId = default(string), string merchant = default(string), string merchantTransactionID = default(string), string transactionType = default(string), string product = default(string), string productId = default(string), string paymentMethod = default(string), string currency = default(string), double? amount = default(double?), string customerName = default(string), string customerEmailAddress = default(string), string customerIPAddress = default(string), string responseCode = default(string), string responseCodeDescription = default(string), string riskScore = default(string), string riskStatus = default(string), double? exchangeRate = default(double?), double? exchangeAmount = default(double?), string exchangeCurrency = default(string))
        {
            Id = id;
            Date = date;
            GlobalId = globalId;
            GlobalProperty = globalProperty;
            BankId = bankId;
            Bank = bank;
            CorporateId = corporateId;
            Corporate = corporate;
            Mid = mid;
            MerchantId = merchantId;
            Merchant = merchant;
            MerchantTransactionID = merchantTransactionID;
            TransactionType = transactionType;
            Product = product;
            ProductId = productId;
            PaymentMethod = paymentMethod;
            Currency = currency;
            Amount = amount;
            CustomerName = customerName;
            CustomerEmailAddress = customerEmailAddress;
            CustomerIPAddress = customerIPAddress;
            ResponseCode = responseCode;
            ResponseCodeDescription = responseCodeDescription;
            RiskScore = riskScore;
            RiskStatus = riskStatus;
            ExchangeRate = exchangeRate;
            ExchangeAmount = exchangeAmount;
            ExchangeCurrency = exchangeCurrency;
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
        [JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

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
        [JsonProperty(PropertyName = "merchantTransactionID")]
        public string MerchantTransactionID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "product")]
        public string Product { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "productId")]
        public string ProductId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "paymentMethod")]
        public string PaymentMethod { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "amount")]
        public double? Amount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "customerName")]
        public string CustomerName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "customerEmailAddress")]
        public string CustomerEmailAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "customerIPAddress")]
        public string CustomerIPAddress { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "responseCode")]
        public string ResponseCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "responseCodeDescription")]
        public string ResponseCodeDescription { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "riskScore")]
        public string RiskScore { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "riskStatus")]
        public string RiskStatus { get; set; }

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

    }
}
