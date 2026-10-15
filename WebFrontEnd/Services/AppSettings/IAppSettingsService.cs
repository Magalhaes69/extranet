using WebFrontEnd.Services.AppSettings.Dtos;

namespace WebFrontEnd.Services.AppSettings
{
    public interface IAppSettingsService
    {
        /// <summary>
        /// Asynchronously retrieves all application settings and returns them as an AppSettingViewDto object.
        /// </summary>
        /// <returns>A task that represents the asynchronous operations. The task result contains a DTO representing the application settings.</returns>
        Task<List<AppSettingViewDto>> GetAllAppSettings();
    }
}
