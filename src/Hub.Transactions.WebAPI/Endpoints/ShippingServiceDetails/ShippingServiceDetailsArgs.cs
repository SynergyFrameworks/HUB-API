using Hub.Transactions.WebAPI.Models;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.ThreeDSecureDetails
{
    public class ShippingServiceDetailsArgs : BasicArgs
    {
        /// <summary>
        /// [Id] of [cs_rpt_timestamp] table
        /// </summary>
        public List<int> IDs { get; set; }
    }
}
