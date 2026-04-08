using WebFrontEnd.Services.Quotations.Dto;

namespace WebFrontEnd.Services.Quotations
{
    public interface IQuotationsService
    {
        Task<List<QuotationsDto>> GetQuotationsAsync(string cardCode, bool forceRefresh = false);
        Task<QuotationsDto> GetByDocEntryQuotationsAsync(string cardCode,int DocEntry, bool forceRefresh = false);
        Action QuotationsAction { get; set; }
    }
}
