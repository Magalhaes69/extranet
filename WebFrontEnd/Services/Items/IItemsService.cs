using WebFrontEnd.Services.Items.DTO;
using WebFrontEnd.Services.Orders.Models;

namespace WebFrontEnd.Services.Items
{
    public interface IItemsService
    {
        Task<ItemsDto> GetByIdAsync(int id);
        Task<List<ItemsDto>> GetItemsAsync();

        Action ItemsAction { get; set; }
    }
}
