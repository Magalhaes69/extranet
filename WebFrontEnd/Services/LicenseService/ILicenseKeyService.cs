using System.ComponentModel;
using WebFrontEnd.Services.LicenseService.Dto;
using WebFrontEnd.Shared.Results;

namespace WebFrontEnd.Services.LicenseService
{
    public interface ILicenseKeyService 
    {
        Task<ApiResponse<PagedObjectResult<LicenseKey>>> ValidateAsync();
    }
}
