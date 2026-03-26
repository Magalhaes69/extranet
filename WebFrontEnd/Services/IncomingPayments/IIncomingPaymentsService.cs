using WebFrontEnd.Services.IncomingPayments.Dto;


namespace WebFrontEnd.Services.IncomingPayments
{
    public interface IIncomingPaymentsService
    {
        Task<List<GetAllIncomingPaymentsDto>> GetIncomingPaymentsServiceAsync(string cardCode, bool forceRefresh = false);
        Action IncomingPaymentsServiceAction { get; set; }
    }
}
