using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Shared.Dtos;


namespace WebFrontEnd.Services.IncomingPayments
{
    public interface IIncomingPaymentsService
    {
        Task<GetIncomingPaymentsResult> GetIncomingPaymentsServiceAsync(IncomingPaymentsRequest request, bool forceRefresh = false);
        Task<ViewDetailsIncomingPaymentsDto> GetByDocNum(GetByDocNumIncomingPaymentsRequest request);
        Action IncomingPaymentsServiceAction { get; set; }
    }
}
