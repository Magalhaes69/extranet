using Application.Dtos.OrdersCards;
using MediatR;

namespace Application.UseCases.Items.GetAllItemsCatalog
{
    public class GetAllItemsCatalogRequest : IRequest<List<CatalogItemDto>>
    {
        public string Valid { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int TPriceBusiness { get; set; }
        public List<int> GroupNumbes { get; set; } = new();
    }
}
