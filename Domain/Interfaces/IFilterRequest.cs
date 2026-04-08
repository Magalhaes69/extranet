using Domain.Helps;

namespace Domain.Interfaces
{
    public interface IFilterRequest
    {
        public List<DynamicFilter>?  DynamicFilters { get; set; }
    }
}
