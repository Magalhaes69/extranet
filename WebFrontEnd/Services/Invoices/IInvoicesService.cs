using WebFrontEnd.Services.Invoices.Dto;
using WebFrontEnd.Shared.Dtos;

namespace WebFrontEnd.Services.Invoices
{
    public interface IInvoicesService
    {
        Task<List<GetAllInvoicesDto>> GetInvoicesAsync(
            InvoiceRequest invoiceRequest,
            bool forceRefresh = false);
        Task<GetAllInvoicesDto> GetByDocEntryInvoicesAsync(string cardCode, int DocEntry, bool forceRefresh = false);
        Action InvoicesAction { get; set; }
    }
}
