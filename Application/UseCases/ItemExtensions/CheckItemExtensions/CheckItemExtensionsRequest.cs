
using MediatR;

namespace Application.UseCases.ItemExtensions.CheckItemExtensions
{
    public sealed record CheckItemExtensionsRequest : IRequest<CheckItemExtensionsResponse>
    {
        public string ItemCode { get; init; } 
    }
}
