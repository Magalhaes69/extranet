using System.Reflection;
using Microsoft.Extensions.Localization;
using WebFrontEnd.Services.Enums;
using WebFrontEnd.Services.Filter;
using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.Help
{
    public  class FilterMetadataBuilder
    {
        private readonly IStringLocalizer<SalesGeneralApp> _L;
        public FilterMetadataBuilder(IStringLocalizer<SalesGeneralApp> localizer)
        {
            _L = localizer;
        }
        public List<FilterField> Build<T>()
        {
            return typeof(T)
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(FilterableAttribute), true).Any())
                .Select(p =>
                {
                    var attr = p.GetCustomAttributes<FilterableAttribute>();
                    return new FilterField
                    {
                        Name = p.Name,
                        Label = attr.FirstOrDefault()!.LabelKey != null 
                        ? _L[attr.FirstOrDefault()!.LabelKey!] 
                        : p.Name,
                        DataType = p.PropertyType,
                        AllowedOperators = GetOperatorsByType(p.PropertyType)
                    };
                }).ToList();
        }

        private List<OperatorType> GetOperatorsByType(Type type)
        {
            if (type == typeof(string))
            {
                return new()
            {
                OperatorType.Iqual,
                OperatorType.NotIqual,
                OperatorType.Contains,
                OperatorType.StartsWith,
                OperatorType.EndsWith
            };
            }

            if (type == typeof(int) || type == typeof(decimal) || type == typeof(DateTime))
            {
                return new()
            {
                OperatorType.Iqual,
                OperatorType.NotIqual,
                OperatorType.GreaterThan,
                OperatorType.GreaterThanOrEqual,
                OperatorType.LessThan,
                OperatorType.LessThanOrEqual,
                OperatorType.Between
            };
            }

            return new();
        }
    }
}
