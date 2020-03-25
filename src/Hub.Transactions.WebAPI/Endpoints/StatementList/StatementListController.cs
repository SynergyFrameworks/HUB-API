using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Endpoints.StatementList
{
    [Route("[controller]")]
    public class StatementListController : Controller
    {
        private readonly IStatementDBExtensions _db;
        public StatementListController(IStatementDBExtensions db)
        {
            _db = db;
        }
        [HttpGet]
        public IEnumerable<StatementListRow> Get([FromQuery]StatementListArgs args)
        {
            var criteria = new List<string>();
            var id = string.Empty;
            switch (args.type)
            {
                case StatementType.Bank:
                    id = "bank_id";
                    break;
                case StatementType.Corporate:
                    id = "corporate_id";
                    break;
                case StatementType.Merchant:
                    id = "merchant_id";
                    break;
                default:
                    break;
            }
            var sql = @"
                SELECT id, date, statement_end_date
                FROM public.pdf_statements where " + id + "=@0";
            var data = _db.Fetch<StatementListRow>(sql, args.key);
            return data;
        }
        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]StatementListArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            return null;
        }
    }
}
