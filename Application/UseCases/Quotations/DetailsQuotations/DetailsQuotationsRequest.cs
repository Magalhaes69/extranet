using Application.UseCases.Quotations.GetAllQuotations;
using MediatR;

namespace Application.UseCases.Quotations.DetailsQuotations
{
    public class DetailsQuotationsRequest : IRequest<GetAllQuotationsResponse>
    {
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
    }
}
