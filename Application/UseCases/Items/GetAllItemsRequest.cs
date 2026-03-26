using MediatR;

namespace Application.UseCases.Items
{
    public class GetAllItemsRequest : IRequest<List<GetAllItemsResponse>>;
}
