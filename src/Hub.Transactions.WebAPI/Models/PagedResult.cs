using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Models
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            Rows = new List<T>();
        }

        public List<T> Rows { get; set; }
        
        public long Count { get; set; }

        public long TotalCount { get; set; }

        public long Page { get; set; }

        public long PageCount { get; set; }

        public long PageSize { get; set; }
    }
}
