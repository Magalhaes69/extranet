using WebFrontEnd.Services.VatGroups.DTO;

namespace WebFrontEnd.Services.VatGroups
{
    public interface IVatGroupsService
    {
        Task<VatGroupsDto> GetByIdAsync(int id);
        Task<List<VatGroupsDto>> GetVatGroupsAsync();

        Action IVatGroupsAction { get; set; }
    }
}
