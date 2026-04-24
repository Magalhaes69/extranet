using Application.UseCases.Quotations.GetAllQuotations;
using MediatR;

namespace Application.UseCases.Quotations.DetailsQuotations
{
    public class DetailsQuotationsRequest : IRequest<DetailsQuotationsResponse>
    {
        public string CardCode { get; set; }
        public int DocEntry { get; set; }
    }
}
