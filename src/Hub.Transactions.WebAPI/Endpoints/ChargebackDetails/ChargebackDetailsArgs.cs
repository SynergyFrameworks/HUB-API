using Hub.Transactions.WebAPI.Models;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.ChargebackDetails
{
    /// <summary>
    /// [Id] of [cs_rpt_timestamp] table
    /// </summary>
    public class ChargebackDetailsArgs : BasicArgs
    {
        public List<int> IDs { get; set; }
    }
}
