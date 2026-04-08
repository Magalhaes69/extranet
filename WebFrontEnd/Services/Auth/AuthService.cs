using Microsoft.AspNetCore.Components;


namespace WebFrontEnd.Services.Auth
{
    public class AuthService
    {
        private const string BaseUrl = "api/Users";
        private readonly NavigationManager _navigationManager;

        private HttpClient _httpClient;
        public AuthService(NavigationManager nav,
           HttpClient clientFactory)
        {
            _navigationManager = nav;
            _httpClient = clientFactory;
        }
        public async Task<AuthResponse> Login(LoginDto login)
        {
            var status = await _httpClient.PostAsync($"{BaseUrl}/login", General.GennerateStringContent(General.SerializeObject(login)));

            if (status.IsSuccessStatusCode)
            {
                var token = await status.Content.ReadAsStringAsync();
                var result = General.DeserialzeJsonString<AuthResponse>(token);

                return result;
            }
            else
            {
                return null!;
            }
        }

        public async Task<AuthResponse> VerifyByCode(Verify2FADto verify2FADto)
        {
            var response = await _httpClient.PostAsync($"{BaseUrl}/verify-2fa", General.GennerateStringContent(General.SerializeObject(verify2FADto)));

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var result = General.DeserialzeJsonString<AuthResponse>(token);

                return result;
            }
            else
            {
                return null!;
            }
        }

        public async Task<AppUserCreateDto> Profile(string email)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/profile?email={email}");

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var result = General.DeserialzeJsonString<AppUserCreateDto>(token);

                return result;
            }
            else
            {
                return null!;
            }
        }

        public async Task<ServiceResponsePost> UpdateProfile(AppUserCreateDto appUserCreateDto)
        {
            var response = await _httpClient.PutAsync($"{BaseUrl}/updateprofile", General.GennerateStringContent(General.SerializeObject(appUserCreateDto)));
            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var result = CheckResponsePost(response);
                if (!result.Flag) return result;
                var apiResponse = await RedContent(response);

                return General.DeserialzeJsonString<ServiceResponsePost>(apiResponse);
            }
            else
            {
                return null!;
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
