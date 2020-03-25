using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Endpoints.StatementList
{
    public class StatementListRow
    {
        public int Id { set; get; }
        public DateTime date { set; get; }
        public DateTime statement_end_date { set; get; }
    }
}
