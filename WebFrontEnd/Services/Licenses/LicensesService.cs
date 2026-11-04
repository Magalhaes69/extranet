using System.Net.Http.Json;
using WebFrontEnd.Services.Licenses.Dto;

namespace WebFrontEnd.Services.Licenses
{
    public class LicensesService : ILicensesService
    {
        private readonly HttpClient _httpClient;

        // Endpoints of the extranet WebAPI (to be connected to the LicenseKey platform in the next step):
        //   GET  api/Licenses                 -> list of the licenses of the logged customer
        //   POST api/Licenses/{id}/renew      -> { months, notes }
        //   POST api/Licenses/request         -> { product, users, notes }
        private const string BaseUrl = "api/Licenses";

        public LicensesService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<List<LicenseItemDto>?> GetAllAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(BaseUrl);
                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<List<LicenseItemDto>>()
                       ?? new List<LicenseItemDto>();
            }
            catch
            {
                return null;
            }
        }

        public Task<ServiceResponsePost> RenewAsync(string id, LicenseRenewFormDto form) =>
            SendAsync($"{BaseUrl}/{Uri.EscapeDataString(id)}/renew", new
            {
                months = form.Months,
                notes = string.IsNullOrWhiteSpace(form.Notes) ? null : form.Notes.Trim()
            });

        public Task<ServiceResponsePost> RequestAsync(LicenseRequestFormDto form) =>
            SendAsync($"{BaseUrl}/request", new
            {
                product = form.Product.Trim(),
                users = form.Users,
                notes = string.IsNullOrWhiteSpace(form.Notes) ? null : form.Notes.Trim()
            });

        /// <summary>
        /// Posts the request and returns the API message (success or business error).
        /// An empty message means an unexpected failure; the page shows its own generic text.
        /// </summary>
        private async Task<ServiceResponsePost> SendAsync(string url, object body)
        {
            try
            {
                using var response = await _httpClient.PostAsJsonAsync(url, body);
                var json = await response.Content.ReadAsStringAsync();

                ServiceResponsePost? result = null;
                try
                {
                    result = General.DeserialzeJsonString<ServiceResponsePost>(json);
                }
                catch
                {
                    // Not our JSON (empty body, error page, ...)
                }

                if (result is not null && !string.IsNullOrWhiteSpace(result.Message))
                {
                    result.Flag = response.IsSuccessStatusCode && result.Flag;
                    return result;
                }

                return new ServiceResponsePost { Flag = response.IsSuccessStatusCode };
            }
            catch
            {
                return new ServiceResponsePost { Flag = false };
            }
        }
    }
}
