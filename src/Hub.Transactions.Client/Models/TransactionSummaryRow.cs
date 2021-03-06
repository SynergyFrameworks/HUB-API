// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class TransactionSummaryRow
    {
        /// <summary>
        /// Initializes a new instance of the TransactionSummaryRow class.
        /// </summary>
        public TransactionSummaryRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the TransactionSummaryRow class.
        /// </summary>
        public TransactionSummaryRow(string apiId = default(string), string product = default(string), string currency = default(string), double? averageAmount = default(double?), double? successRate = default(double?), int? totalVolume = default(int?), double? averageSeconds = default(double?), int? totalCountry = default(int?), System.DateTime? lastDate = default(System.DateTime?))
        {
            ApiId = apiId;
            Product = product;
            Currency = currency;
            AverageAmount = averageAmount;
            SuccessRate = successRate;
            TotalVolume = totalVolume;
            AverageSeconds = averageSeconds;
            TotalCountry = totalCountry;
            LastDate = lastDate;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "apiId")]
        public string ApiId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "product")]
        public string Product { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currency")]
        public string Currency { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "averageAmount")]
        public double? AverageAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "successRate")]
        public double? SuccessRate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "totalVolume")]
        public int? TotalVolume { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "averageSeconds")]
        public double? AverageSeconds { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "totalCountry")]
        public int? TotalCountry { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "lastDate")]
        public System.DateTime? LastDate { get; set; }

    }
}
