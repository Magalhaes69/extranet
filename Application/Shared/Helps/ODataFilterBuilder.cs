using Application.UseCases.Items.GetAllItemsCatalog;
using Domain.Helps;
using Domain.Helps.Enum;
using Domain.Interfaces;

namespace Application.Shared.Helps
{
    public class ODataFilterBuilder
    {
        public static string Build<T>(T request) where T : IFilterRequest
        {
            var filters = new List<string>();

            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                if (prop.Name == nameof(IFilterRequest.DynamicFilters))
                    continue;

                var value = prop.GetValue(request);
                if (value == null) continue;

                // List<string> → OR
                if (value is IEnumerable<string> list && prop.PropertyType != typeof(string))
                {
                    if(prop.Name != "Page" && prop.Name != "PageSize")
                        AddOr(filters, prop.Name, list);
                }
                else
                {
                    if (prop.Name != "Page" && prop.Name != "PageSize")
                        AddEqual(filters, prop.Name, value);
                }
            }

           
            if (request.DynamicFilters?.Any() == true)
            {
                var grouped = request.DynamicFilters.GroupBy(f => f.Field);

                var groupedConditions = new List<string>();

                foreach (var group in grouped)
                {
                    var conditions = group
                        .Select(BuildDynamicCondition)
                        .Where(x => !string.IsNullOrEmpty(x));

                    if (conditions.Any())
                    {
                        groupedConditions.Add($"{string.Join(" or ", conditions)}");
                    }
                }

                if (groupedConditions.Any())
                {
                    filters.Add(string.Join(" and ", groupedConditions));
                }
            }

            return filters.Any()
                ? string.Join(" and ", filters)
                : string.Empty;
        }
        //public static string Build(GetAllInvoicesRequest request)
        //{
        //    var filters = new List<string>();

        //    // Simple fields
        //    AddEqual(filters, "CardCode", request.CardCode);
        //    AddOr(filters, "DocumentStatus", request.DocumentStatuses);
        //    AddOr(filters, "Cancelled", request.Cancelled);

        //    // Dynamic filters (NEW ENGINE)
        //    if (request.DynamicFilters?.Any() == true)
        //    {
        //        var grouped = request.DynamicFilters
        //                .GroupBy(f => f.Field);

        //        var groupedConditions = new List<string>();

        //        foreach (var group in grouped)
        //        {
        //            var conditions = group
        //                .Select(BuildDynamicCondition)
        //                .Where(x => !string.IsNullOrEmpty(x));

        //            if (conditions.Any())
        //            {
        //                // SAME FIELD → OR
        //                groupedConditions.Add($"{string.Join(" or ", conditions)}");
        //            }
        //        }

        //        if (groupedConditions.Any())
        //        {
        //            // DIFFERENT FIELDS → AND
        //            filters.Add(string.Join(" and ", groupedConditions));
        //        }
        //    }

        //    return filters.Any()
        //        ? string.Join(" and ", filters)
        //        : string.Empty;
        //}

        // =============================
        // BASIC HELPERS
        // =============================

        private static void AddEqual(List<string> filters, string field, object? value)
        {
            if (value == null) return;

            if (value is string str && string.IsNullOrWhiteSpace(str))
                return;

            filters.Add($"{field} eq {FormatValue(value)}");
        }

        private static void AddOr(List<string> filters, string field, IEnumerable<string>? values)
        {
            if (values?.Any() != true) return;

            var orFilter = string.Join(" or ",
                values.Select(v => $"{field} eq {FormatValue(v)}"));

            filters.Add($"({orFilter})");
        }

        // =============================
        // DYNAMIC FILTER ENGINE
        // =============================

        private static string BuildDynamicCondition(DynamicFilter f)
        {
            return f.Operator switch
            {
                OperatorType.Iqual =>
                    $"{f.Field} eq {FormatValue(f.Value1)}",

                OperatorType.NotIqual =>
                    $"{f.Field} ne {FormatValue(f.Value1)}",

                OperatorType.GreaterThan =>
                    $"{f.Field} gt {FormatValue(f.Value1)}",

                OperatorType.GreaterThanOrEqual =>
                    $"{f.Field} ge {FormatValue(f.Value1)}",

                OperatorType.LessThan =>
                    $"{f.Field} lt {FormatValue(f.Value1)}",

                OperatorType.LessThanOrEqual =>
                    $"{f.Field} le {FormatValue(f.Value1)}",

                OperatorType.Between =>
                    $"({f.Field} ge {FormatValue(f.Value1)} and {f.Field} le {FormatValue(f.Value2)})",
                
                    OperatorType.Contains =>
                                        $"contains({f.Field}, {FormatValue(f.Value1)})",

            OperatorType.StartsWith =>
                      $"startswith({f.Field}, {FormatValue(f.Value1)})",

            OperatorType.EndsWith => 
                  $"endswith({f.Field}, {FormatValue(f.Value1)})",

                OperatorType.IsNull =>
                    $"{f.Field} eq null",

                OperatorType.IsNotNull =>
                    $"{f.Field} ne null",

                _ => ""
            };
        }

        // =============================
        // FORMAT (VERY IMPORTANT)
        // =============================

        private static string FormatValue(object? value)
        {
            if (value == null)
                return "null";

            // Date
            if (value is DateTime dt)
                return dt.ToString("yyyy-MM-dd");

            // STRING handling (important)
            if (value is string str)
            {
                // Try parse number
                if (int.TryParse(str, out var intValue))
                    return intValue.ToString();

                if (decimal.TryParse(str, out var decValue))
                    return decValue.ToString();

                // Try parse date
                if (DateTime.TryParse(str, out var dateValue))
                    return dateValue.ToString("yyyy-MM-dd");

                // Default → string
                return $"'{str}'";
            }

            // Number (real numeric types)
            if (value is int or decimal or double)
                return value.ToString()!;

            return $"'{value}'";
        }

        public static string BuildCatalog(GetAllItemsCatalogRequest request)
        {
            var filters = new List<string>();

            if (request.GroupNumbes != null && request.GroupNumbes.Any())
            {
                var groupFilter = string.Join(" or ",
                    request.GroupNumbes.Select(id => $"ItemsGroupCode eq {id}"));

                filters.Add($"({groupFilter})");
            }

            return string.Join(" and ", filters);
        }
    }
}
