using WebFrontEnd.Services.VatGroups.DTO;
using WebFrontEnd.Services.Warehouses.Dto;

namespace WebFrontEnd.Services.Warehouses
{
    public interface IWarehousesService
    {
        Task<WarehousesDto> GetByCodeAsync(string code);
        Task<List<WarehousesDto>> GetWarehousesServiceAsync(string Inactive);

        Action WarehousesDtoAction { get; set; }
    }
}
