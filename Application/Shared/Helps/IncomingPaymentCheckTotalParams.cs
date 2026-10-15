using Application.UseCases.IncomingPayments.GetAllIncomingPayments;
using Domain.Helps;
using Domain.Helps.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.Helps
{
    public static class IncomingPaymentCheckTotalParams
    {
        private const string MinDate = "1900-01-01";
        private const string MaxDate = "9999-12-31";
        private static readonly string MaxInt = int.MaxValue.ToString();

        private enum Kind { Text, Date, Number }

        private sealed record Field(string Param, Kind Kind, bool Nullable = false, Func<string, string>? Map = null);

        // OData field name (as sent in DynamicFilters) -> query parameter
        private static readonly Dictionary<string, Field> Fields = new(StringComparer.OrdinalIgnoreCase)
        {
            ["CardCode"] = new("cardCode", Kind.Text, Nullable: true),
            ["CardName"] = new("cardName", Kind.Text, Nullable: true),
            ["ProjectCode"] = new("project", Kind.Text, Nullable: true),
            ["Cancelled"] = new("cancelled", Kind.Text, Map: v => v switch
            {
                "tYES" => "Y",
                "tNO" => "N",
                _ => v
            }),
            ["DocType"] = new("docType", Kind.Text, Map: v => v switch
            {
                "rCustomer" => "C",
                "rSupplier" => "S",
                "rAccount" => "A",
                _ => v
            }),
            ["DocCurrency"] = new("currency", Kind.Text),
            ["DocDate"] = new("date", Kind.Date),
            ["DueDate"] = new("due", Kind.Date),
            ["TaxDate"] = new("tax", Kind.Date),
            ["DocNum"] = new("docNum", Kind.Number),
        };

        /// <summary>
        /// Builds the ParamList values for the IncomingPaymentsCheckTotal query.
        /// Returns false when the filter can't be expressed by the query
        /// (unknown field, several filters on one field, unsupported operator).
        /// </summary>
        public static bool TryBuild(GetAllIncomingPaymentsRequest request, out Dictionary<string, string> p)
        {
            p = Defaults();

            // Fixed request properties (same as ODataFilterBuilder)
            if (!string.IsNullOrWhiteSpace(request.CardCode))
                SetText(p, Fields["CardCode"], request.CardCode);

            var cancelled = request.Cancelled?.Where(v => !string.IsNullOrWhiteSpace(v)).Distinct().ToList();
            if (cancelled?.Count == 1)
                SetText(p, Fields["Cancelled"], cancelled[0]);
            // 0 or 2 values (tYES + tNO) -> no filter, same result as OData

            if (request.DynamicFilters is null || request.DynamicFilters.Count == 0)
                return true;

            foreach (var group in request.DynamicFilters.GroupBy(f => f.Field))
            {
                if (!Fields.TryGetValue(group.Key, out var field))
                    return false; // field not covered by the query

                var filters = group.ToList();

                // OData ORs several filters on the same field -> not expressible with one range/pattern
                if (filters.Count > 1)
                    return false;

                var ok = field.Kind == Kind.Text
                    ? ApplyText(p, field, filters[0])
                    : ApplyRange(p, field, filters[0]);

                if (!ok) return false;
            }

            return true;
        }

        private static Dictionary<string, string> Defaults() => new()
        {
            ["cardCode"] = "%",
            ["cardCodeNull"] = "0",
            ["cardName"] = "%",
            ["cardNameNull"] = "0",
            ["project"] = "%",
            ["projectNull"] = "0",
            ["cancelled"] = "%",
            ["docType"] = "%",
            ["currency"] = "%",
            ["dateFrom"] = MinDate,
            ["dateTo"] = MaxDate,
            ["dueFrom"] = MinDate,
            ["dueTo"] = MaxDate,
            ["taxFrom"] = MinDate,
            ["taxTo"] = MaxDate,
            ["docNumFrom"] = "0",
            ["docNumTo"] = MaxInt
        };

        private static void SetText(Dictionary<string, string> p, Field field, string value)
        {
            p[field.Param] = field.Map is null ? value : field.Map(value);

            // When filtering, rows with NULL in this column must be excluded
            if (field.Nullable)
                p[field.Param + "Null"] = MaxInt;
        }

        private static bool ApplyText(Dictionary<string, string> p, Field field, DynamicFilter f)
        {
            var v = f.Value1 ?? string.Empty;
            var mapped = field.Map is null ? v : field.Map(v);

            switch (f.Operator)
            {
                case OperatorType.Iqual:
                    SetText(p, field with { Map = null }, mapped);
                    return true;
                case OperatorType.Contains:
                    SetText(p, field with { Map = null }, $"%{mapped}%");
                    return true;
                case OperatorType.StartsWith:
                    SetText(p, field with { Map = null }, $"{mapped}%");
                    return true;
                case OperatorType.EndsWith:
                    SetText(p, field with { Map = null }, $"%{mapped}");
                    return true;
                default:
                    return false; // NotIqual, IsNull, ranges on text...
            }
        }

        private static bool ApplyRange(Dictionary<string, string> p, Field field, DynamicFilter f)
        {
            var from = field.Param + "From";
            var to = field.Param + "To";

            string? Norm(string? raw, int shift = 0)
            {
                if (field.Kind == Kind.Date)
                    return DateTime.TryParse(raw, out var d) ? d.Date.AddDays(shift).ToString("yyyy-MM-dd") : null;

                return int.TryParse(raw, out var n) ? (n + shift).ToString() : null;
            }

            switch (f.Operator)
            {
                case OperatorType.Iqual:
                    var eq = Norm(f.Value1);
                    if (eq is null) return false;
                    p[from] = eq;
                    p[to] = eq;
                    return true;

                case OperatorType.GreaterThanOrEqual: return Set(p, from, Norm(f.Value1));
                case OperatorType.GreaterThan: return Set(p, from, Norm(f.Value1, +1));
                case OperatorType.LessThanOrEqual: return Set(p, to, Norm(f.Value1));
                case OperatorType.LessThan: return Set(p, to, Norm(f.Value1, -1));
                case OperatorType.Between: return Set(p, from, Norm(f.Value1)) && Set(p, to, Norm(f.Value2));

                default:
                    return false;
            }
        }

        private static bool Set(Dictionary<string, string> p, string key, string? value)
        {
            if (value is null) return false;
            p[key] = value;
            return true;
        }
    }
}
