using System.Collections.Generic;
using Hub.Transactions.WebAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionDetails
{
    [Route("[controller]")]
    public class TransactionDetailsController : Controller
    {
        private readonly IDatabase _db;

        public TransactionDetailsController(IDatabase db)
        {
            _db = db;
        }

        [HttpGet]
        public List<TransactionDetailsRow> Get([FromQuery] TransactionDetailsArgs args)
        {
            var criteria = new List<string>();

            criteria.Add("tx.mongo_m_id = tmpRelated.mongo_m_id");
            criteria.Add("tx.m_txn_id = tmpRelated.m_txn_id");

            var where = criteria.ToWhereClause();

            var sql = $@"
                    WITH tmpCancel AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                        WHERE tx.resp_code = '1000' 
                        AND tx.txn_type = 'CancelPayment'
                    GROUP BY tx.m_txn_id,
                        tx.mongo_m_id),

                    tmpCapture AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    WHERE tx.resp_code IN ('1000', '1042', '1116')
                        AND tx.txn_type = 'CapturePayment'
                    GROUP BY tx.m_txn_id,
                        tx.mongo_m_id),

                    tmpRefund AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        COALESCE(SUM(tx.amount), 0) as netamt
                    FROM cs_rpt_txn tx
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id
                    WHERE tx.resp_code IN ('1000', '1042', '1116')
                        AND (tx.txn_type = 'RefundPayment' OR (tx.txn_type = 'PreApproveRefund' AND tx.pre_approval_status != 'PROCESSED'))
                    GROUP BY tx.m_txn_id,
                        tx.mongo_m_id),

                    tmpRelated AS 
                    (SELECT tx.mongo_m_id, 
                        tx.m_txn_id 
                    FROM cs_rpt_txn tx 
                        LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                    WHERE ts.id = @ID),

                    tmpFirstTxn AS
                    (SELECT tx.m_txn_id,
                        tx.mongo_m_id,
                        tx.amount
                    FROM cs_rpt_txn tx
                        LEFT JOIN tmpRelated ON tmpRelated.m_txn_id = tx.m_txn_id AND tmpRelated.mongo_m_id = tx.mongo_m_id
                    WHERE tx.m_txn_id = tmpRelated.m_txn_id
                    and tx.resp_code IN ('1000', '1042', '1116') AND tx.txn_type IN('AuthPayment', 'OneStepPayment', 'OnlinePayment') 
                    order by tx.req_rcv_at_main_flw asc 
                    limit 1)

                    SELECT
                    COALESCE(ts.provider_reference, '') AS ProviderReference, 
                    ts.id AS ID, 
                    tx.original_payment_id AS OriginalPaymentID,
                    tx.apc_txn_id AS APCTransactionID,
                    tx.req_rcv_at_main_flw + (@TimeZoneOffset || ' minutes')::interval AS Date, 
                    tx.mongo_m_id AS MerchantId, 
                    tx.m_txn_id AS MerchantTransactionID, 
                    tx.mid AS MID, 
                    tx.channel_type AS ChannelType,
                    tx.amount*.001 AS Amount, 
                    COALESCE(tx.currency, '') AS Currency, 
                    COALESCE(tx.txn_type, '') AS TransactionType, 
                    COALESCE(ts.api_id, '') AS ProductId, 
                    COALESCE(ts.api_name, '') AS Product, 
                    COALESCE(tx.resp_code, '') AS ResponseCode, 
                    COALESCE(ts.provider_resp_code, '') AS ProviderResponse, 
                    ts.res_rcv_frm_ext_api + (@TimeZoneOffset || ' minutes')::interval AS ResponseTimestamp, 
                    EXTRACT(EPOCH FROM ts.res_rcv_frm_ext_api - ts.req_snt_to_ext_api)::numeric AS ProcessingTime, 
                    COALESCE(tx.customer_email_address, '') AS CustomerEmailAddress, 
                    COALESCE(tx.billing_address_customer_name, '') AS BillingAddressCustomerName, 
                    COALESCE(tx.billing_address_street1, '') AS BillingAddressStreet1, 
                    COALESCE(tx.billing_address_city, '') AS BillingAddressCity, 
                    COALESCE(tx.billing_address_country, '') AS BillingAddressCountry, 
                    COALESCE(tx.shipping_address_customer_name, '') AS ShippingAddressCustomerName, 
                    COALESCE(tx.shipping_address_street1, '') AS ShippingAddressStreet1, 
                    COALESCE(tx.shipping_address_city, '') AS ShippingAddressCity, 
                    COALESCE(tx.shipping_address_country, '') AS ShippingAddressCountry, 
                    COALESCE(tx.customer_tel_no, '') AS CustomerTelNo, 
                    COALESCE(tx.customer_id, '') AS CustomerID, 
                    COALESCE(tx.customer_ip_address, '') AS CustomerIPAddress, 
                    COALESCE(tx.customer_date_of_birth, '') AS CustomerDateOfBirth, 
                    COALESCE(tx.customer_social_id, '') AS CustomerSocialID,
                    COALESCE(tx.method_name, '') AS PaymentMethod,
                    COALESCE(tx.card_type, '') AS CardType,
                    COALESCE(tx.apc_resp_data, '') AS TransactionResponse,
                    ts.provider_resp_code AS ProviderResponseCode,
                    ts.provider_resp_code_desc AS ProviderResponseCodeDescription,
                    tx.pre_approval_status AS PreApprovalStatus,
					case 
                        when (COALESCE(tmpRefund.netamt, 0) > 0 AND (tx.resp_code IN ('1000', '1042') AND tx.txn_type IN('CapturePayment','OneStepPayment')))
                            then (tx.amount - COALESCE(tmpRefund.netamt, 0) - COALESCE(tmpCancel.netamt, 0)) > 0
                        when (COALESCE(tmpRefund.netamt, 0) = 0 AND (tx.resp_code IN ('1000', '1042') AND tx.txn_type IN('CapturePayment','OneStepPayment')))
                            then (tx.amount - COALESCE(tmpCancel.netamt, 0)) > 0
                        when (tx.resp_code = '1000' AND tx.txn_type IN('AuthPayment', 'IncrementalAuthPayment', 'OnlinePayment'))
                            then (tx.amount - COALESCE(tmpCapture.netamt, 0) - COALESCE(tmpCancel.netamt, 0)) > 0
                        when
                            (tx.resp_code = '1042' AND tx.txn_type ='RefundPayment')
                        then (tmpFirstTxn.amount - COALESCE(tmpCancel.netamt, 0) - COALESCE(tmpRefund.netamt, 0)) > 0
                    end
                    AS EligibleForCancel,
                    case 
                        when (COALESCE(tmpRefund.netamt, 0) > 0 AND tx.txn_type IN('CapturePayment','OneStepPayment'))
                            then (tx.amount - COALESCE(tmpRefund.netamt, 0) - COALESCE(tmpCancel.netamt, 0))*.001
                        when (COALESCE(tmpRefund.netamt, 0) = 0 AND tx.txn_type IN('CapturePayment','OneStepPayment'))
                            then (tx.amount - COALESCE(tmpCancel.netamt, 0))*.001
                        when (tx.txn_type IN('AuthPayment', 'IncrementalAuthPayment', 'OnlinePayment'))
                            then (tx.amount - COALESCE(tmpCapture.netamt, 0) - COALESCE(tmpCancel.netamt, 0))*.001
                        when
                            (tx.resp_code = '1042' AND tx.txn_type ='RefundPayment')
                        then (tmpFirstTxn.amount - COALESCE(tmpCancel.netamt, 0) - COALESCE(tmpRefund.netamt, 0))*.001
                    end
                    AS  AmountEligibleForCancel,
                    tx.resp_code = '1000' AND tx.txn_type IN ('AuthPayment','IncrementalAuthPayment') AND 
                    ((tx.amount - COALESCE(tmpCancel.netamt, 0) - COALESCE(tmpCapture.netamt, 0)) > 0) AS EligibleForCapture,
                    (tx.amount - COALESCE(tmpCapture.netamt, 0) - COALESCE(tmpCancel.netamt, 0))*.001 as AmountEligibleForCapture,
                    case
                        when tx.resp_code IN('1000', '1116') AND tx.txn_type IN ('CapturePayment', 'OnlinePayment', 'OneStepPayment')
                            then (tx.amount - COALESCE(tmpRefund.netamt, 0) - COALESCE(tmpCancel.netamt, 0)) > 0
                        end
                    AS EligibleForRefund,
                    case
                        when (COALESCE(tmpRefund.netamt, 0)) = 0 then (tx.amount)*.001 
                        when (COALESCE(tmpRefund.netamt, 0)) > 0 AND tx.resp_code IN('1000', '1116') AND tx.txn_type IN ('CapturePayment', 'OnlinePayment', 'OneStepPayment')
                            then (tx.amount - COALESCE(tmpRefund.netamt, 0) - COALESCE(tmpCancel.netamt, 0))*.001 
                    end AS AmountEligibleForRefund 

                    FROM cs_rpt_txn tx 
                    LEFT JOIN cs_rpt_timestamp ts ON tx.cs_rpt_txn_id = ts.cs_rpt_txn_id 
                    LEFT JOIN tmpRelated ON tmpRelated.m_txn_id = tx.m_txn_id AND tmpRelated.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpFirstTxn ON tmpFirstTxn.m_txn_id = tx.m_txn_id AND tmpFirstTxn.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpCancel ON tmpCancel.m_txn_id = tx.m_txn_id AND tmpCancel.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpCapture ON tmpCapture.m_txn_id = tx.m_txn_id AND tmpCapture.mongo_m_id = tx.mongo_m_id
                    LEFT JOIN tmpRefund ON tmpRefund.m_txn_id = tx.m_txn_id AND tmpRefund.mongo_m_id = tx.mongo_m_id
                {where}";

            var result = _db.Fetch<TransactionDetailsRow>(sql, args);

            return result;
        }
    }
}
