using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Shared.Dtos;


namespace WebFrontEnd.Services.IncomingPayments
{
    public interface IIncomingPaymentsService
    {
        Task<GetIncomingPaymentsResult> GetIncomingPaymentsServiceAsync(IncomingPaymentsRequest request, bool forceRefresh = false);
        Action IncomingPaymentsServiceAction { get; set; }
    }
}
