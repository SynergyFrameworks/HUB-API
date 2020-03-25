// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Hub.Transactions.Client.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class AuthorisationByCountrySummaryRow
    {
        /// <summary>
        /// Initializes a new instance of the AuthorisationByCountrySummaryRow
        /// class.
        /// </summary>
        public AuthorisationByCountrySummaryRow() { }

        /// <summary>
        /// Initializes a new instance of the AuthorisationByCountrySummaryRow
        /// class.
        /// </summary>
        public AuthorisationByCountrySummaryRow(string product = default(string), string corporateId = default(string), string corporate = default(string), string mid = default(string), string country = default(string), DateTimeOffset? date = default(DateTimeOffset?), string cardType = default(string), int? authCount = default(int?), int? declineCount = default(int?))
        {
            Product = product;
            CorporateId = corporateId;
            Corporate = corporate;
            Mid = mid;
            Country = country;
            Date = date;
            CardType = cardType;
            AuthCount = authCount;
            DeclineCount = declineCount;
        }

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
        [JsonProperty(PropertyName = "mid")]
        public string Mid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public DateTimeOffset? Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "cardType")]
        public string CardType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "authCount")]
        public int? AuthCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "declineCount")]
        public int? DeclineCount { get; set; }

    }
}
