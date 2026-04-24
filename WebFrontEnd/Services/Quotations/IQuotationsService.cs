using WebFrontEnd.Services.Quotations.Dto;
using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.Quotations
{
    public interface IQuotationsService
    {
        Task<List<QuotationsDto>> GetQuotationsAsync(
            QuotationRequest request, 
            bool forceRefresh = false);
        Task<QuotationsDto> GetByDocEntryQuotationsAsync(string cardCode,int DocEntry, bool forceRefresh = false);
        Action QuotationsAction { get; set; }
    }
}
