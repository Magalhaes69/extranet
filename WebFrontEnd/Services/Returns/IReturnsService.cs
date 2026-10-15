using WebFrontEnd.Services.IncomingPayments.Dto;
using WebFrontEnd.Services.Returns.Dto;

namespace WebFrontEnd.Services.Returns
{
    public interface IReturnsService
    {
        Task<GetAllReturnsResultDto> GetAllAsync(GetAllReturnsRequestDto request, bool forceRefresh = false);
        Task<ViewDetailsIncomingPaymentsDto> GetByDocNum(GetByDocNumIncomingPaymentsRequest request);
        Action ReturnsServiceAction { get; set; }
    }
}
