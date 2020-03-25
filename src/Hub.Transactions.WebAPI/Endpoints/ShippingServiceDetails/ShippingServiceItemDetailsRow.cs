using System;

namespace Hub.Transactions.WebAPI.Endpoints.ThreeDSecureDetails
{
    public class ShippingServiceItemDetailsRow
    {
        internal long ShipmentDetailID { get; set; }

        public string ItemId { get; set; }

        public string ProductId { get; set; }

        public string ConsignmentId { get; set; }

        public string BarCodeId { get; set; }

        public string TotalCost { get; set; }

        public string TotalTax { get; set; }

        public string ItemStatus { get; set; }

        public string ItemCode { get; set; }

        public string ArticleId { get; set; }

        public string ProductName { get; set; }

        public string EstimatedDelivery { get; set; }

        public string LeavePackage { get; set; }

        public string SignatureRequired { get; set; }

        public string TotalBundledCost { get; set; }

        public string TotalBundledTax { get; set; }

        public DateTime EarliestDeliveryDate { get; set; }

        public DateTime LatestDeliveryDate { get; set; }

        public string RecommendedAmount { get; set; }

        public string EventLocation { get; set; }

        public string EventDescription { get; set; }

        public DateTime EventDate { get; set; }

        public string ProductType { get; set; }

        public string TrackingURL { get; set; }

        public string TerviceType { get; set; }

    }
}
