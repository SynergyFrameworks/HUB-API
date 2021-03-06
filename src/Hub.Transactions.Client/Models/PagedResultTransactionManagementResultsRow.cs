// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Hub.Transactions.Client.Models
{
    using Hub.Transactions;
    using Hub.Transactions.Client;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class PagedResultTransactionManagementResultsRow
    {
        /// <summary>
        /// Initializes a new instance of the
        /// PagedResultTransactionManagementResultsRow class.
        /// </summary>
        public PagedResultTransactionManagementResultsRow()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the
        /// PagedResultTransactionManagementResultsRow class.
        /// </summary>
        public PagedResultTransactionManagementResultsRow(IList<TransactionManagementResultsRow> rows = default(IList<TransactionManagementResultsRow>), long? count = default(long?), long? totalCount = default(long?), long? page = default(long?), long? pageCount = default(long?), long? pageSize = default(long?))
        {
            Rows = rows;
            Count = count;
            TotalCount = totalCount;
            Page = page;
            PageCount = pageCount;
            PageSize = pageSize;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "rows")]
        public IList<TransactionManagementResultsRow> Rows { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public long? Count { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "totalCount")]
        public long? TotalCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "page")]
        public long? Page { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "pageCount")]
        public long? PageCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "pageSize")]
        public long? PageSize { get; set; }

    }
}
