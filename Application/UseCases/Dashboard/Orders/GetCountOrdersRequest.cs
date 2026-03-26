using MediatR;

namespace Application.UseCases.Dashboard.Orders
{
    public class GetCountOrdersRequest : IRequest<GetCountResponse>
    {
        public string Status { get; set; }
        public string CardCode { get; set; }
    }
}
