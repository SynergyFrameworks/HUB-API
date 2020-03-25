using System;
using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Models
{
    public class BasicArgs
    {
        public BasicArgs()
        {
            PageSize = 500;
            Page = 1;
        }

        public int PageSize { get; set; }

        public int Page { get; set; }

    }
}
