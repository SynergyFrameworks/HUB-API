using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionDetails
{
    public class TransactionDetailsRow
    {
        public int ID { get; set; }

        public string OriginalPaymentID { get; set; }

        public string MerchantTransactionID { get; set; }

        public string APCTransactionID { get; set; }

        public DateTime? Date { get; set; }

        public string MID { get; set; }

        public string ChannelType { get; set; }

        public string MerchantId { get; set; }

        public string ProviderReference { get; set; }

        public string TransactionType { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string ProductId { get; set; }

        public string Product { get; set; }

        public string ResponseCode { get; set; }

        public string ProviderResponse { get; set; }

        public DateTime? ResponseTimestamp { get; set; }

        public double ProcessingTime { get; set; }

        public string CustomerEmailAddress { get; set; }

        public string BillingAddressCustomerName { get; set; }

        public string BillingAddressStreet1 { get; set; }

        public string BillingAddressCity { get; set; }

        public string BillingAddressCountry { get; set; }

        public string ShippingAddressCustomerName { get; set; }

        public string ShippingAddressStreet1 { get; set; }

        public string ShippingAddressCity { get; set; }

        public string ShippingAddressCountry { get; set; }

        public string CustomerTelNo { get; set; }

        public string CustomerID { get; set; }

        public string CustomerIPAddress { get; set; }

        public string CustomerDateOfBirth { get; set; }

        public string CustomerSocialID { get; set; }

        public string PreApprovalStatus { get; set; }

        public bool EligibleForCancel { get; set; }

        public bool EligibleForCapture { get; set; }

        public bool EligibleForRefund { get; set; }

        public decimal AmountEligibleForCancel { get; set; }

        public decimal AmountEligibleForCapture { get; set; }

        public decimal AmountEligibleForRefund { get; set; }

        public string PaymentMethod { get; set; }

        public string CardType { get; set; }

        public string TransactionResponse { get; set; }

        public string ProviderResponseCode { get; set; }

        public string ProviderResponseCodeDescription { get; set; }
    }
}
