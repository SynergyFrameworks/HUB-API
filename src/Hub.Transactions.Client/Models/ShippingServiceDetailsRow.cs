// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class ShippingServiceDetailsRow
    {
        /// <summary>
        /// Initializes a new instance of the ShippingServiceDetailsRow class.
        /// </summary>
        public ShippingServiceDetailsRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the ShippingServiceDetailsRow class.
        /// </summary>
        public ShippingServiceDetailsRow(long? shipmentDetailID = default(long?), long? transactionID = default(long?), string shipmentId = default(string), string shipmentReference = default(string), string action = default(string), string status = default(string), string shipmentOrderId = default(string), System.DateTime? shipmentDate = default(System.DateTime?), string requestId = default(string), string labelStatus = default(string), string labelURL = default(string), string totalCost = default(string), string totalTax = default(string), string redirectURL = default(string), string numberOfItem = default(string), string shippingCarrier = default(string), IList<ShippingServiceItemDetailsRow> items = default(IList<ShippingServiceItemDetailsRow>))
        {
            ShipmentDetailID = shipmentDetailID;
            TransactionID = transactionID;
            ShipmentId = shipmentId;
            ShipmentReference = shipmentReference;
            Action = action;
            Status = status;
            ShipmentOrderId = shipmentOrderId;
            ShipmentDate = shipmentDate;
            RequestId = requestId;
            LabelStatus = labelStatus;
            LabelURL = labelURL;
            TotalCost = totalCost;
            TotalTax = totalTax;
            RedirectURL = redirectURL;
            NumberOfItem = numberOfItem;
            ShippingCarrier = shippingCarrier;
            Items = items;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "shipmentDetailID")]
        public long? ShipmentDetailID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "transactionID")]
        public long? TransactionID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "shipmentId")]
        public string ShipmentId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "shipmentReference")]
        public string ShipmentReference { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "action")]
        public string Action { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "shipmentOrderId")]
        public string ShipmentOrderId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "shipmentDate")]
        public System.DateTime? ShipmentDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "labelStatus")]
        public string LabelStatus { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "labelURL")]
        public string LabelURL { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "totalCost")]
        public string TotalCost { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "totalTax")]
        public string TotalTax { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "redirectURL")]
        public string RedirectURL { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "numberOfItem")]
        public string NumberOfItem { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "shippingCarrier")]
        public string ShippingCarrier { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "items")]
        public IList<ShippingServiceItemDetailsRow> Items { get; set; }

    }
}