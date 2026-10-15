
namespace Application.UseCases.Items
{
    public sealed record GetAllItemsResult
    {
        public List<GetAllItemsResponse> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
