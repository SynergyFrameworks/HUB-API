using Hub.Transactions.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Endpoints.StatementData
{
    public class StatementDataArgs : Args
    {
        public int ID { get; set; }
    }
}
