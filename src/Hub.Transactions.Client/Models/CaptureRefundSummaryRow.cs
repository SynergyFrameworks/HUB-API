// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CaptureRefundSummaryRow
    {
        /// <summary>
        /// Initializes a new instance of the CaptureRefundSummaryRow class.
        /// </summary>
        public CaptureRefundSummaryRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CaptureRefundSummaryRow class.
        /// </summary>
        public CaptureRefundSummaryRow(string product = default(string), string corporateId = default(string), string corporate = default(string), string bankId = default(string), string bank = default(string), string binCountry = default(string), string mid = default(string), System.DateTime? date = default(System.DateTime?), string cardType = default(string), string transactionType = default(string), string currency = default(string), int? count = default(int?), double? totalAmount = default(double?), string paymentMethod = default(string), double? exchangeRate = default(double?), double? exchangeAmount = default(double?), string exchangeCurrency = default(string))
        {
            Product = product;
            CorporateId = corporateId;
            Corporate = corporate;
            BankId = bankId;
            Bank = bank;
            BinCountry = binCountry;
            Mid = mid;
            Date = date;
            CardType = cardType;
            TransactionType = transactionType;
            Currency = currency;
            Count = count;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
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
        [JsonProperty(PropertyName = "product")]
        public string Product { get; set; }

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
        [JsonProperty(PropertyName = "bankId")]
        public string BankId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "bank")]
        public string Bank { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "binCountry")]
        public string BinCountry { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "mid")]
        public string Mid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cardType")]
        public string CardType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionType")]
        public string TransactionType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "totalAmount")]
        public double? TotalAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "paymentMethod")]
        public string PaymentMethod { get; set; }

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
