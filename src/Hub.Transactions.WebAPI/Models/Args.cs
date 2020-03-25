using System;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Models
{
    public class Args : BasicArgs
    {
        public Args()
            : base()
        {
            OrderBys = new List<string>();
        }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public double TimeZoneOffset { get; set; }

        public List<string> OrderBys { get; set; }
    }
}
