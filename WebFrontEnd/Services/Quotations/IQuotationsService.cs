using WebFrontEnd.Services.Quotations.Dto;
using WebFrontEnd.Shared.Dtos;
using WebFrontEnd.Shared.Results;

namespace WebFrontEnd.Services.Quotations
{
    public interface IQuotationsService
    {
        Task<PagedResult<QuotationsDto, TotalResultDto>> GetQuotationsAsync(
            QuotationRequest request, 
            bool forceRefresh = false);
        Task<QuotationsDto> GetByDocEntryQuotationsAsync(string cardCode,int DocEntry, bool forceRefresh = false);
        Action QuotationsAction { get; set; }
    }
}
