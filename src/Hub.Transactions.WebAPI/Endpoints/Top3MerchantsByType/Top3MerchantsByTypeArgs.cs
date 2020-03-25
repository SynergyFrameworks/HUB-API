using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Endpoints.Top3MerchantsByType
{
    public class Top3MerchantsByTypeArgs : BasicArgs
    {
        /// <summary>
        /// [mid] of [cs_rpt_txn] table
        /// </summary>
        public string MID { get; set; }
    }
}
