using WebFrontEnd.Services.Enums;

namespace WebFrontEnd.Services.Filter
{
    public class FilterField
    {
        public string Name { get; set; } = "";
        public string Label { get; set; } = "";
        public Type DataType { get; set; } = typeof(string);

        public List<OperatorType> AllowedOperators { get; set; } = new();
    }
}
