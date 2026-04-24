
using MediatR;

namespace Application.UseCases.ItemExtensions.DeleteItemExtensions
{
    public sealed record DeleteItemExtensionRequest : IRequest<DeleteItemExtensionResponse>
    {
        public List<string> ItemCode { get; init; }
    }
}
