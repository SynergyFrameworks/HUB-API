using Hub.Transactions.WebAPI.Extensions;
using Hub.Transactions.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hub.Transactions.WebAPI.Endpoints.StatementData
{
    [Route("[controller]")]
    public class StatementDataController : Controller
    {
        private readonly IStatementDBExtensions _db;
        public StatementDataController(IStatementDBExtensions db)
        {
            _db = db;
        }

        [HttpGet]
        public IEnumerable<StatementDataRow> Get([FromQuery]StatementDataArgs args)
        {
            //var criteria = new List<string>();
            //var id = string.Empty;
            //switch (args.type)
            //{
            //    case StatementType.Bank:
            //        id = "bank_id";
            //        break;
            //    case StatementType.Corporate:
            //        id = "corporate_id";
            //        break;
            //    case StatementType.Merchant:
            //        id = "merchant_id";
            //        break;
            //    default:
            //        break;
            //}
            var sql = @"
                SELECT file_name, pdf_file
                FROM public.pdf_statements where id=@0";
            var data = _db.Fetch<StatementDataRow>(sql, args.ID);
            return data;
        }

        [HttpGet]
        [Route("[action]")]
        public IEnumerable<FilterOption> Options([FromQuery]StatementDataArgs args, [FromQuery]OptionArgs optionsArgs)
        {
            return null;
        }


    }
}
