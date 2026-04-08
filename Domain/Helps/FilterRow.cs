

namespace Domain.Helps
{
    public class FilterRow
    {
        public FilterType Type { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? Today { get; set; }
        public string? Value { get; set; }
    }
}
