
using MediatR;

namespace Application.UseCases.ItemExtensions.GetByItemIdItemExtensions
{
    public sealed record GetByItemIdItemExtensionsRequest : IRequest<GetByItemIdItemExtensionsResponse>
    {
        public string ItemId { get; set; }
    }
}
