

using MediatR;

namespace Application.UseCases.OrdersCarts.GetLastOrderCartPend
{
    public class GetLastOrdersCartPendRequest : IRequest<GetLastOrdersCartPendResponse>
    {
        public string CardCode { get; set; }
        public string UserId { get; set; }
    }
}
