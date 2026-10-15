using WebFrontEnd.Services.VatGroups.DTO;
using WebFrontEnd.Services.Warehouses.Dto;

namespace WebFrontEnd.Services.Warehouses
{
    public interface IWarehousesService
    {
        Task<WarehousesDto> GetByCodeAsync(string code);
        Task<WarehousesResultDto> GetWarehousesServiceAsync(
            WarehousesRequestDto request,
            bool forceRefresh = false);

        Action WarehousesDtoAction { get; set; }
    }
}
