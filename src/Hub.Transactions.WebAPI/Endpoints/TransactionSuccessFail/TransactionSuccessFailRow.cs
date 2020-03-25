using System;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionSuccessFail
{
    public class TransactionSuccessFailRow
    {
        public string ProductId { get; set; }

        public string Product { get; set; }

        public string MerchantId { get; set; }

        public string MID { get; set; }

        public string CorporateId { get; set; }

        public string Corporate { get; set; }

        public DateTime Date { get; set; }

        public string TransactionType { get; set; }

        public string PaymentMethod { get; set; }

        public string CardType { get; set; }

        public int SuccessCount { get; set; }

        public int FailCount { get; set; }
    }
}
