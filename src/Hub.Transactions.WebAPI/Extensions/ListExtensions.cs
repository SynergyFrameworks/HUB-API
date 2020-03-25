using System;
using System.Collections.Generic;
using System.Linq;

namespace Hub.Transactions.WebAPI.Extensions
{
    public static class ListExtensions
    {
        public static void AddIfNotNull<T>(this List<T> list, object nullCandidate, T item)
        {
            AddIfNotNull(list, nullCandidate, null, null, item, null);
        }

        public static void AddIfNotNull<T>(this List<T> list, object nullCandidate, T item, Action action)
        {
            AddIfNotNull(list, nullCandidate, null, null, item, action);
        }

        public static void AddIfNotNull<T>(this List<T> list, object nullCandidate, string filterProperty, string optionProperty, T item)
        {
            AddIfNotNull(list, nullCandidate, filterProperty, optionProperty, item, null);
        }

        public static void AddIfNotNull<T>(this List<T> list, object nullCandidate, string filterProperty, string optionProperty, T item, Action action)
        {
            if (nullCandidate != null)
            {
                var exclude = filterProperty != null && optionProperty != null && filterProperty == optionProperty;

                if (!exclude)
                {
                    list.Add(item);
                    action?.Invoke(); 
                }
            }
        }

        public static string ToWhereClause<T>(this List<T> list)
        {
            if (list != null && list.Any())
            {
                return "WHERE " + string.Join(" AND ", list);
            }

            return "WHERE true";
        }
    }
}