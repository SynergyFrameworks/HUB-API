using System;
using System.Collections.Generic;
using System.Linq;
using Hub.Transactions.WebAPI.Models;

namespace Hub.Transactions.WebAPI.Extensions
{
    public static class StringExtensions
    {
        public static string ToFirstUpper(this string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            var result = char.ToUpper(source[0]) + source.Substring(1);

            return result;
        }

        public static string ToFirstOnlyUpper(this string source)
        {
            if (string.IsNullOrEmpty(source)) return source;

            var result = char.ToUpper(source[0]) + source.Substring(1).ToLower();

            return result;
        }

        public static bool IsNullOrEmpty(this string source)
        {
            var result = string.IsNullOrEmpty(source);

            return result;
        }

        public static bool NotNullOrEmpty(this string source)
        {
            var result = source.IsNullOrEmpty() == false;

            return result;
        }

        public static bool ToBool(this string source)
        {
            bool result;
            bool.TryParse(source, out result);

            return result;
        }

        public static string MapToColumn(this string propertyName)
        {
            switch (propertyName)
            {
                case "Global": return "tx.global_name";
                case "GlobalId": return "tx.global_id";
                case "Bank": return "tx.bank_name";
                case "BankId": return "tx.mongo_bank_id";
                case "CorporateId": return "tx.corp_id";
                case "Corporate": return "tx.corp_name";
                case "MerchantId": return "tx.mongo_m_id";
                case "Merchant": return "tx.merchant_name";
                case "MID": return "tx.mid";
                case "Product": return "ts.api_name";
                case "APIId": return "ts.api_id";
                case "CardType": return "tx.card_type";
                case "ResponseCode": return "tx.resp_code";
                case "ResponseCodeDescription": return "ts.resp_code_desc";
                case "BIN": return "tx.Bin";
                case "BINCountry": return "tx.bin_country";
                case "PaymentMethod": return "tx.method_name";
                case "TransactionType": return "tx.txn_type";
                case "Currency": return "tx.currency";
                case "ExchangeCurrency": return "tx.exch_currency";
                case "Country": return "tx.billing_address_country";
                case "AccountHolder": return "tx.acc_holder";
                case "ProviderResponseCode": return "ts.provider_resp_code";
                case "TransactionSource": return "tx.requestor_origin";

                default: return null;
            }
        }
        
        public static string ToOptionsSQL(this string sql, OptionArgs optionArgs)
        {
            return ToOptionsSQL(sql, optionArgs, MapToColumn);
        }

        public static string ToOptionsSQL(this string sql, OptionArgs optionArgs, Func<string, string> columnMapper)
        {
            var valueColumn = columnMapper(optionArgs.OptionValue);
            var textColumn = columnMapper(optionArgs.OptionText);

            var selStart = sql.LastIndexOf("SELECT ", StringComparison.CurrentCultureIgnoreCase);   //This is a problem because it prevents using subqueries in the SQL statement. A better approach could be instead of using a single method to generate the entire SQL statement (Esample: [WebAPI.Endpoints.Transactions.TransactionsController.GetSQL()]), using several methods and that each one be responsible for generating a part of the statement, then you could call the method that generates the necessary part, and avoid having to find the "SELECT" with a "LastIndexOf"
            var selEnd = sql.LastIndexOf(" FROM ", StringComparison.CurrentCultureIgnoreCase);
            var orderByStart = sql.LastIndexOf(" ORDER BY ", StringComparison.CurrentCultureIgnoreCase);
            var len = orderByStart > selEnd ? orderByStart - selEnd : sql.Length - selEnd;
            var tmpTable = sql.Substring(0, selStart);
            var fromWhere = sql.Substring(selEnd, len);

            var optionsSQL = $@"{tmpTable}
                            SELECT DISTINCT
                                {valueColumn} AS Key,
                                {textColumn} AS Value 
                            {fromWhere}
                                AND {valueColumn} <> '' 
                                AND {textColumn} <> ''
                            ORDER BY {textColumn}";

            return optionsSQL;
        }
        public static string ToOrderByClause(this List<string> input, string defaultOrder = null)
        {
            var output = new List<string>();

            if (input.Empty() && defaultOrder.NotNullOrEmpty())
            {
                input.Add(defaultOrder);
            }

            foreach (var orderBy in input)
            {
                var a = orderBy.Split(',').Select(x => x.Trim()).Where(x => x.NotNullOrEmpty());

                output.AddRange(a);
            }

            var values = output.Select(x =>
            {
                var parts = x.Split(' ');
                var column = parts[0].ToLower();
                var hasDirection = parts.Length == 2;
                var direction = hasDirection ? parts[1].ToUpper() : "ASC";

                var orderBy = $"{column} {direction}";

                return orderBy;
            });

            var orderByValues = string.Join($", ", values);
            var clause = output.Any() ? $"ORDER BY {orderByValues}" : String.Empty;

            return clause;
        }

        public static bool Empty(this IEnumerable<object> candidate)
        {
            return candidate.Any() == false;
        }
    }
}
