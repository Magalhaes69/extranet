using WebFrontEnd.Services.Invoices.Dto;


namespace WebFrontEnd.Services.Invoices
{
    public interface IInvoicesService
    {
        Task<List<GetAllInvoicesDto>> GetInvoicesAsync(string cardCode, bool forceRefresh = false);
        Action InvoicesAction { get; set; }
    }
}
