using Hub.Transactions.WebAPI.Models;
using PetaPoco;
using System;

namespace Hub.Transactions.WebAPI.Extensions
{
    public static class PageExtensions
    {
        public static PagedResult<T> ToPagedResult<T>(this Page<T> page)
        {
            var result = new PagedResult<T>
            {
                Count = page.Items.Count,
                Page = page.CurrentPage,
                PageCount = page.TotalPages,
                PageSize = page.ItemsPerPage,
                TotalCount = page.TotalItems,
                Rows = page.Items 
            };

            return result;
        }

        public static PagedResult<T> FetchPagedResult<T>(this IDatabase database, long page, long itemsPerPage, string sql, params object[] args)
        {
            Page<T> petaPocoPage;

            if (sql.IndexOf("group by", StringComparison.OrdinalIgnoreCase) != -1)
            {
                var sqlPage = string.Format("{0} LIMIT {1} OFFSET {2}", sql, itemsPerPage, itemsPerPage * (page - 1));
                var sqlCount = string.Format("SELECT COUNT(*) FROM({0}) AS TOTAL", sql);
                petaPocoPage = database.Page<T>(page, itemsPerPage, sqlCount, args, sqlPage, args);
            }
            else
            {
                petaPocoPage = database.Page<T>(page, itemsPerPage, sql, args);
            }

            var result = petaPocoPage.ToPagedResult();

            return result;
        }
    }
}