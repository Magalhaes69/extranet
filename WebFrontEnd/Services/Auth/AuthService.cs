using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;


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

        public async Task<ServiceResponsePost> UpdateProfile(
            AppUserCreateDto appUserCreateDto, 
            IBrowserFile? file)
        {
            try
            {
                var content = new MultipartFormDataContent();

                content.Add(new StringContent(appUserCreateDto.Email ?? ""), "Email");
                content.Add(new StringContent(appUserCreateDto.Id ?? ""), "Id");
                content.Add(new StringContent(appUserCreateDto.FirstName ?? ""), "FirstName");
                content.Add(new StringContent(appUserCreateDto.LastName ?? ""), "LastName");
                content.Add(new StringContent(appUserCreateDto.Nuit ?? ""), "Nuit");
                content.Add(new StringContent(appUserCreateDto.CardCode ?? ""), "CardCode");
                content.Add(new StringContent(appUserCreateDto.Theme ?? ""), "Theme");
                content.Add(new StringContent(appUserCreateDto.PhoneNumber ?? ""), "PhoneNumber");
                content.Add(new StringContent(appUserCreateDto.Language ?? ""), "Language");
                content.Add(new StringContent(appUserCreateDto.ImageUrl ?? ""), "ImageUrl");
                content.Add(new StringContent(appUserCreateDto.TwoFactorEnabled.ToString() ?? "false"), "TwoFactorEnabled");

                if (file != null)
                {
                    var fileContent = new StreamContent(file.OpenReadStream(5 * 1024 * 1024));

                    fileContent.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    content.Add(fileContent, "file", file.Name);
                }

                var response = await _httpClient.PutAsync($"{BaseUrl}/updateprofile", content);
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
            catch (Exception ex)
            {
                return null!;
            }
        }

        public async Task<AppUserViewDto> GetUserById(string Id)
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/getbyid?Id={Id}");

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                var result = General.DeserialzeJsonString<AppUserViewDto>(token);

                return result;
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
