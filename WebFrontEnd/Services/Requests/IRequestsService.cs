using WebFrontEnd.Services.Requests.Dtos;

namespace WebFrontEnd.Services.Requests
{
    public interface IRequestsService
    {
        Task<List<RequestListItemDto>> GetAllRequestsAsync(CancellationToken ct = default);
        Task<RequestDetailsDto?> GetRequestDetailsAsync(Guid id, CancellationToken ct = default);
        Task<bool> CreateRequestAsync(CreateRequestDto dto, CancellationToken ct = default);
        Task<bool> CancelRequestAsync(Guid id, CancellationToken ct = default);
    }
}
