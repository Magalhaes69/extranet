using Microsoft.AspNetCore.Components.Forms;
using WebFrontEnd.Services.ItemExtension.Dto;

namespace WebFrontEnd.Services.ItemExtension
{
    public interface IItemExtensionService
    {
        Task<ServiceResponseGeneralPost<ViewItemExtensionDto>> CreateAsync(CreateItemExtensionDto create, IBrowserFile? file);
        Task<ServiceResponseGeneralPost<ViewItemExtensionDto>> EditAsync(EditItemExtensionDto create, IBrowserFile? file);
        Task<bool> CheckByItemCodeAsync(string ItemCode);

        Task<List<ViewItemExtensionDto>> GetAll();
        Task<ViewItemExtensionDto> GetByItemId(string ItemId);

        Task<ServiceResponseGeneralPost<object>> RemoveAsync(List<string> ItemCode);
    }
}
