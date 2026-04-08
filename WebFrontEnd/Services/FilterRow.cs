using WebFrontEnd.Services.Enums;

namespace WebFrontEnd.Services
{
    public class FilterRow
    {
        public FilterType Type { get; set; }
        public OperatorType Operator { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Today { get; set; }
        public string? Value { get; set; }
        public int? DocNum1 { get; set; }
        public int? DocNum2 { get; set; }


    }
}
