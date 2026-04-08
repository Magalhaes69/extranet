using MediatR;

namespace Application.UseCases.Quotations.GetAllQuotations
{
    public record GetAllQuotationsRequest : IRequest<List<GetAllQuotationsResponse>>
    {
        public string CardCode { get; set; }
        public DateTime? DocDate { get; set; }
        public string? Series { get; set; }
    }
}
