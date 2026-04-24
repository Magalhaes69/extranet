
using MediatR;

namespace Application.UseCases.ItemExtensions.GetAllItemExtensions
{
    public sealed record GetAllItemExtensionsRequest : IRequest<List<GetAllItemExtensionsResponse>>
    {
    }
}
