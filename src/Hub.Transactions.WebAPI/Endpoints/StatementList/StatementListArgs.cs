using Hub.Transactions.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Endpoints.StatementList
{
    public class StatementListArgs : Args
    {
        public string key { get; set; }
        public string type { get; set; }
    }
}
