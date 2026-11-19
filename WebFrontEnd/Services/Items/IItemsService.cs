using WebFrontEnd.Services.Items.DTO;

namespace WebFrontEnd.Services.Items
{
    public interface IItemsService
    {
        Task<List<CatalogItemDto>> GetFirstPageAsync(
            GetAllitemsCatalogRequest request);
        Task GetBackgroundAsync(
           GetAllitemsCatalogRequest request,
           Action<List<CatalogItemDto>> onPageLoaded,
           CancellationToken cancellationToken);
        Task<ItemsResultDto> GetItemsAsync(
            GetAllItemsRequest request,
            bool forceRefresh = false);

        Action ItemsAction { get; set; }
    }
}
