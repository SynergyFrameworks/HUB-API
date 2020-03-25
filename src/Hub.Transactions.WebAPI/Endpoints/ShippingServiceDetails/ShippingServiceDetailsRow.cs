using System;

namespace Hub.Transactions.WebAPI.Endpoints.ThreeDSecureDetails
{
    public class ShippingServiceDetailsRow
    {
        public long ShipmentDetailID { get; set; }

        public long TransactionID { get; set; }

        public string ShipmentId { get; set; }

        public string ShipmentReference { get; set; }

        public string Action { get; set; }

        public string Status { get; set; }

        public string ShipmentOrderId { get; set; }

        public DateTime ShipmentDate { get; set; }

        public string RequestId { get; set; }

        public string LabelStatus { get; set; }

        public string LabelURL { get; set; }

        public string TotalCost { get; set; }

        public string TotalTax { get; set; }

        public string RedirectURL { get; set; }
        
        public string NumberOfItem { get; set; }

        public string ShippingCarrier { get; set; }
               
        public ShippingServiceItemDetailsRow[] Items { get; set; }
    }
}
