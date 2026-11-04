using System.Net.Http.Json;
using WebFrontEnd.Services.Leads.Dto;

namespace WebFrontEnd.Services.Leads
{
    public class LeadsService : ILeadsService
    {
        private readonly HttpClient _httpClient;

        // Endpoint of the extranet WebAPI that forwards the new account to the leads API.
        // It must allow anonymous calls (the visitor is not logged in yet).
        // The connection is the next step; until then the call simply fails with a generic error.
        private const string BaseUrl = "api/Leads";

        // A new account starts a trial. Set here, never typed by the visitor.
        private const int TrialDays = 30;
        private const int TrialStatus = 0;

        public LeadsService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("API");
        }

        public async Task<ServiceResponsePost> CreateAsync(LeadFormDto lead)
        {
            var today = DateTime.UtcNow.Date;

            var body = new
            {
                companyName = lead.CompanyName.Trim(),
                nuit = lead.Nuit.Trim(),
                contactName = lead.ContactName.Trim(),
                email = lead.Email.Trim(),
                phoneNumber = lead.PhoneNumber.Trim(),
                trialStartDate = today,
                trialEndDate = today.AddDays(TrialDays),
                status = TrialStatus,
                notes = string.IsNullOrWhiteSpace(lead.Notes) ? null : lead.Notes.Trim(),
                password = lead.Password
            };

            try
            {
                using var response = await _httpClient.PostAsJsonAsync(BaseUrl, body);
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
