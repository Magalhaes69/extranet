using System.ComponentModel;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WebFrontEnd.Services.LicenseService.Dto;
using WebFrontEnd.Shared.Results;

namespace WebFrontEnd.Services.LicenseService
{
    public class LicenseKeyService : ILicenseKeyService
    {
        private readonly HttpClient _httpClient;
        private readonly LicenseSettings _license;
        public LicenseKeyService(HttpClient httpClient, IOptions<LicenseSettings> license)
        {
            _httpClient = httpClient;
           _license = license.Value;
        }
        public async Task<ApiResponse<PagedObjectResult<LicenseKey>>> ValidateAsync()
        {
            try
            {
                var key = _license.LicenseKey;
                var url = _license.LicenseServerUrl;

                var request = new LicenseContextRequest
                {
                    Key = key
                };

                var serialized = General.GennerateStringContent(General.SerializeObject(request));
                var response = await _httpClient.PostAsync(
                    url,
                   serialized);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<PagedObjectResult<LicenseKey>>()
                    {
                        Success = false,
                        Message = "Error occured. Try again later...",
                        Data = null
                    };
                }

                var result = CheckResponsePost(response);



                var apiResponse = await RedContent(response);

                return General.DeserialzeJsonString<ApiResponse<PagedObjectResult<LicenseKey>>>(apiResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<PagedObjectResult<LicenseKey>>()
                {
                    Success = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
            }
        }

        private ServiceResponsePost CheckResponsePost(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                return new ServiceResponsePost()
                {
                    Flag = false,
                    Message = "Error occured. Try again later..."
                };
            }

            return new ServiceResponsePost()
            {
                Flag = true,
                Message = null!
            };
        }

        private static async Task<string> RedContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();
    }
}

