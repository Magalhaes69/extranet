using WebFrontEnd.Services.Enums;

namespace WebFrontEnd.Services.Filter
{
    public class DynamicFilter
    {
        public string Field { get; set; } = "";
        public OperatorType Operator { get; set; }
        public string? Value1 { get; set; }     
        public string? Value2 { get; set; }
        public LogicalOperator Logic { get; set; } = LogicalOperator.And;
    }
}
