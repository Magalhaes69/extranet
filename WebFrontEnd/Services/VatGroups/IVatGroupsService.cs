using WebFrontEnd.Services.VatGroups.DTO;

namespace WebFrontEnd.Services.VatGroups
{
    public interface IVatGroupsService
    {
        Task<VatGroupsDto> GetByIdAsync(int id);
        Task<VatGroupsResultDto> GetVatGroupsAsync(
            VatGroupsRequestDto request,
            bool forceRefresh = false);

        Action IVatGroupsAction { get; set; }
    }
}
