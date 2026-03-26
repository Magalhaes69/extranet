using Microsoft.AspNetCore.Components;


namespace WebFrontEnd.Services.Auth
{
    public class AuthService
    {
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
            var status = await _httpClient.PostAsync("api/Users/login", General.GennerateStringContent(General.SerializeObject(login)));

            if (status.IsSuccessStatusCode)
            {
                var token = await status.Content.ReadAsStringAsync();
                var result =  General.DeserialzeJsonString<AuthResponse>(token);

                return result;
            }
            else
            {
                return null!;
            }
        }
    }
}
