// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Hub.Transactions.Client.Models
{
    using Newtonsoft.Json;
    using System.Linq;

    public partial class StatementListRow
    {
        /// <summary>
        /// Initializes a new instance of the StatementListRow class.
        /// </summary>
        public StatementListRow()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the StatementListRow class.
        /// </summary>
        public StatementListRow(int? id = default(int?), System.DateTime? date = default(System.DateTime?), System.DateTime? statementEndDate = default(System.DateTime?))
        {
            Id = id;
            Date = date;
            StatementEndDate = statementEndDate;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "date")]
        public System.DateTime? Date { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "statement_end_date")]
        public System.DateTime? StatementEndDate { get; set; }

    }
}