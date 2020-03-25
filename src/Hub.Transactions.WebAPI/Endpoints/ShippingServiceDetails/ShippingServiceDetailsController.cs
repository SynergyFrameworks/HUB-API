using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System.Collections.Generic;
using System.Linq;

namespace Hub.Transactions.WebAPI.Endpoints.ThreeDSecureDetails
{
    [Route("[controller]")]
    public class ShippingServiceDetailsController : Controller
    {
        private readonly IDatabase _db;

        public ShippingServiceDetailsController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public PagedResult<ShippingServiceDetailsRow> Get([FromQuery]ShippingServiceDetailsArgs args)
        {
            var criteria = new List<string>();

            criteria.AddIfNotNull(args.IDs, "ts.id IN (@IDs)");

            var where = criteria.ToWhereClause();

            var sql = @"
                SELECT 
                    sd.id as ShipmentDetailID, 
                    sd.cs_rpt_txn_id AS TransactionID, 
                    sd.shipment_id AS ShipmentId, 
                    sd.shipment_reference AS ShipmentReference, 
                    sd.action AS Action, 
                    sd.status AS Status, 
                    sd.shipment_order_id AS ShipmentOrderId, 
                    sd.shipment_date AS ShipmentDate, 
                    sd.request_id AS RequestId, 
                    sd.label_status AS LabelStatus, 
                    sd.label_url AS LabelURL, 
                    sd.total_cost AS TotalCost, 
                    sd.total_tax AS TotalTax, 
                    sd.redirect_url AS RedirectURL,
                    sd.number_of_item AS NumberOfItem, 
                    sd.shipping_carrier AS ShippingCarrier
                FROM shipment_detail sd INNER JOIN
                     cs_rpt_timestamp ts ON sd.cs_rpt_txn_id = ts.cs_rpt_txn_id
                " + where;

            var result = _db.FetchPagedResult<ShippingServiceDetailsRow>(args.Page, args.PageSize, sql, args);

            if (result.Count > 0)
            {
                var sqlItems = @"
                    SELECT 
                        shipment_detail_id AS ShipmentDetailID, 
                        item_id AS ItemId, 
                        product_id AS ProductId, 
                        consignment_id AS ConsignmentId, 
                        bar_code_id AS BarCodeId, 
                        total_cost AS TotalCost, 
                        total_tax AS TotalTax, 
                        item_status AS ItemStatus, 
                        item_code AS ItemCode, 
                        article_id AS ArticleId, 
                        product_name AS ProductName, 
                        estimated_delivery AS EstimatedDelivery, 
                        leave_package AS LeavePackage, 
                        signature_required AS SignatureRequired, 
                        total_bundled_cost AS TotalBundledCost, 
                        total_bundled_tax AS TotalBundledTax, 
                        earliest_delivery_date AS EarliestDeliveryDate, 
                        latest_delivery_date AS LatestDeliveryDate, 
                        recommended_amount AS RecommendedAmount, 
                        event_location AS EventLocation, 
                        event_description AS EventDescription, 
                        event_date AS EventDate, 
                        product_type AS ProductType, 
                        tracking_url AS TrackingURL,
                        service_type AS TerviceType
                    FROM shipment_item_detail
                    WHERE shipment_detail_id IN (@ShipmentDetailIDs) ";

                var resultItems = _db.Fetch<ShippingServiceItemDetailsRow>(sqlItems, new { ShipmentDetailIDs = result.Rows.Select(x => x.ShipmentDetailID).ToList() });

                foreach (var detail in result.Rows)
                    detail.Items = resultItems.FindAll(x => x.ShipmentDetailID == detail.ShipmentDetailID).ToArray();
            }

            return result;
        }
    }
}
