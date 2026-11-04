using WebFrontEnd.Services.Licenses.Dto;

namespace WebFrontEnd.Services.Licenses
{
    public interface ILicensesService
    {
        /// <summary>Returns null when the list could not be loaded.</summary>
        Task<List<LicenseItemDto>?> GetAllAsync();

        Task<ServiceResponsePost> RenewAsync(string id, LicenseRenewFormDto form);

        Task<ServiceResponsePost> RequestAsync(LicenseRequestFormDto form);
    }
}
