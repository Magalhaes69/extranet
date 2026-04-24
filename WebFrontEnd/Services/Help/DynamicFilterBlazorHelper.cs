using Microsoft.Extensions.Localization;
using WebFrontEnd.Services.Enums;
using WebFrontEnd.Services.Filter;

namespace WebFrontEnd.Services.Help
{
    public class DynamicFilterBlazorHelper
    {
        private readonly IStringLocalizer<App> _L;
        public DynamicFilterBlazorHelper(IStringLocalizer<App> L)
        {
            this._L = L;
        }
        public string GetOperatorText(OperatorType operatorType)
        {
            return operatorType switch
            {
                OperatorType.Iqual => _L["iqual"],
                OperatorType.NotIqual => _L["notIqual"],
                OperatorType.GreaterThan => _L["greaterThan"],
                OperatorType.GreaterThanOrEqual => _L["greaterThanOrEqual"],
                OperatorType.LessThan => _L["lessThan"],
                OperatorType.LessThanOrEqual => _L["lessThanOrEqual"],
                OperatorType.Between => _L["between"],
                OperatorType.Contains => _L["contains"],
                OperatorType.StartsWith => _L["startsWith"],
                OperatorType.EndsWith => _L["endsWith"],
                _ => operatorType.ToString()
            };
        }

        // =============================
        // Operators by Field
        // =============================
        public List<SelectItem<OperatorType>> GetOperators(
            string field,
            List<FilterField> availableFields)
        {
            var selectedField = availableFields.FirstOrDefault(f => f.Name == field);

            if (selectedField == null)
                return new();

            return selectedField.AllowedOperators
                .Select(op => new SelectItem<OperatorType>
                {
                    Value = op,
                    Text = GetOperatorText(op)
                })
                .ToList();
        }

        // =============================
        // Render Filter Text
        // =============================
        public string RenderFilterText(
            DynamicFilter f,
            List<FilterField> availableFields)
        {
            var fieldLabel = availableFields
                .FirstOrDefault(x => x.Name == f.Field)?.Label ?? f.Field;

            return f.Operator switch
            {
                OperatorType.Iqual => $"{fieldLabel} = {f.Value1}",
                OperatorType.NotIqual => $"{fieldLabel} ≠ {f.Value1}",
                OperatorType.GreaterThan => $"{fieldLabel} > {f.Value1}",
                OperatorType.GreaterThanOrEqual => $"{fieldLabel} >= {f.Value1}",
                OperatorType.LessThan => $"{fieldLabel} < {f.Value1}",
                OperatorType.LessThanOrEqual => $"{fieldLabel} <= {f.Value1}",
                OperatorType.Contains => $"{fieldLabel} {_L["contains"]} '{f.Value1}'",
                OperatorType.StartsWith => $"{fieldLabel} {_L["startsWith"]} '{f.Value1}'",
                OperatorType.EndsWith => $"{fieldLabel} {_L["endsWith"]} '{f.Value1}'",
                OperatorType.Between => $"{fieldLabel}: {f.Value1} - {f.Value2}",
                _ => $"{fieldLabel}: {f.Value1}"
            };
        }

        // =============================
        // Get Field Label
        // =============================
        public string GetFieldLabel(
            string? field,
            List<FilterField> availableFields)
        {
            if (string.IsNullOrEmpty(field))
                return string.Empty;

            return availableFields
                .FirstOrDefault(f => f.Name == field)?.Label ?? field;
        }

        public string GetInputType(FilterField field)
        {
            if (field.DataType == typeof(int) ||
                field.DataType == typeof(decimal) ||
                field.DataType == typeof(double))
                return "number";

            if (field.DataType == typeof(DateTime) ||
                field.DataType == typeof(DateTime?))
                return "date";

            return "text";
        }

        public List<SelectItem<string>> GetFieldOption(
            string? field,
            List<FilterField> availableFields)
        {
            var f = availableFields.FirstOrDefault(x => x.Name == field);

            if (f == null)
                return new();

            return new List<SelectItem<string>>
                {
                    new SelectItem<string>
                    {
                        Value = f.Name,
                        Text = f.Label
                    }
                };
        }

    }
}
