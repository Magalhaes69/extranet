
using Domain.Helps.Enum;

namespace Domain.Helps
{
    public sealed record DynamicFilter
    {
        public string Field { get; set; } = "";
        public OperatorType Operator { get; set; }
        public string? Value1 { get; set; }
        public string? Value2 { get; set; }
        public LogicalOperator Logic { get; set; } = LogicalOperator.And;
    }
}
