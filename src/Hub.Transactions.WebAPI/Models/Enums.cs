using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Models
{
    public struct StatementType
    {
        public const string Bank = "Banks";
        public const string Corporate = "Corporates";
        public const string Merchant = "Merchants";
    }
}
