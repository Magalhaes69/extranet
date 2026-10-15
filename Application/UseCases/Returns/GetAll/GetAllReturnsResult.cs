

namespace Application.UseCases.Returns.GetAll
{
    public sealed class GetAllReturnsResult
    {
        public List<GetAllReturnsResponse>? Items { get; set; }
        public int TotalCount { get; set; }
    }
}
