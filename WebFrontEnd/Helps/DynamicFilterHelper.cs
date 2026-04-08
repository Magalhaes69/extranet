
using WebFrontEnd.Services.Enums;
using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Helps
{
    public static class DynamicFilterHelper
    {
        public static IEnumerable<T> ApplyFilters<T>(
         IEnumerable<T> items,
         List<DynamicFilter>? filters)
        {
            if (filters == null || !filters.Any())
                return items;

            var grouped = filters.GroupBy(f => f.Field);

            var result = items;

            foreach (var group in grouped)
            {
                var field = group.Key;
                var groupFilters = group.ToList();
                result = result.Where(item => groupFilters.Any(f => Match(item, f)));
            }
            return result;
        }

        private static bool Match<T>(T item, DynamicFilter f)
        {
            var prop = typeof(T).GetProperty(f.Field);
            if (prop == null) return false;

            var value = prop.GetValue(item)?.ToString() ?? "";
            var v1 = f.Value1 ?? "";
            var v2 = f.Value2 ?? "";

            return f.Operator switch
            {
                OperatorType.Iqual =>
                    value.Equals(v1, StringComparison.OrdinalIgnoreCase),

                OperatorType.NotIqual =>
                    !value.Equals(v1, StringComparison.OrdinalIgnoreCase),

                OperatorType.Contains =>
                    value.Contains(v1, StringComparison.OrdinalIgnoreCase),

                OperatorType.StartsWith =>
                    value.StartsWith(v1, StringComparison.OrdinalIgnoreCase),

                OperatorType.EndsWith =>
                    value.EndsWith(v1, StringComparison.OrdinalIgnoreCase),

                OperatorType.GreaterThan =>
                    string.Compare(value, v1, StringComparison.OrdinalIgnoreCase) > 0,

                OperatorType.GreaterThanOrEqual =>
                    string.Compare(value, v1, StringComparison.OrdinalIgnoreCase) >= 0,

                OperatorType.LessThan =>
                    string.Compare(value, v1, StringComparison.OrdinalIgnoreCase) < 0,

                OperatorType.LessThanOrEqual =>
                    string.Compare(value, v1, StringComparison.OrdinalIgnoreCase) <= 0,

                OperatorType.Between =>
                    string.Compare(value, v1, StringComparison.OrdinalIgnoreCase) >= 0 &&
                    string.Compare(value, v2, StringComparison.OrdinalIgnoreCase) <= 0,

                _ => true
            };
        }

        private static object? ConvertValue(object? value, Type targetType)
        {
            if (value == null) return null;

            if (targetType == typeof(DateTime))
                return DateTime.Parse(value.ToString()!);

            if (targetType == typeof(int))
                return int.Parse(value.ToString()!);

            if (targetType == typeof(decimal))
                return decimal.Parse(value.ToString()!);

            return value;
        }

        private static int Compare(object? a, object? b)
        {
            if (a is IComparable compA && b is not null)
                return compA.CompareTo(b);

            return 0;
        }
    }
}
