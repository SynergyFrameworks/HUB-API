using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Endpoints.StatementData
{
    public class StatementDataRow
    {
        public string file_name { get; set; }
        public byte[] pdf_file { get; set; }
    }
}
