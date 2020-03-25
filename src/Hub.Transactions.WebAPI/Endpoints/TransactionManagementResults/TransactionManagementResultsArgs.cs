using System;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Endpoints.TransactionManagementResults
{
    public class TransactionManagementResultsArgs
    {
        public IEnumerable<int> IDs { get; set; }
    }
}
