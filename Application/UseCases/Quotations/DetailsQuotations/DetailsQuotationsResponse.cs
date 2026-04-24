
using Application.Dtos;

namespace Application.UseCases.Quotations.DetailsQuotations
{
    public record DetailsQuotationsResponse : SalesHeader
    {
        public List<GetAllDocumentLinesResponse>? DocumentLines { get; set; }
    }
}
