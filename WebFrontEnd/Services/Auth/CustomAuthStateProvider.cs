using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;



namespace WebFrontEnd.Services.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ISyncLocalStorageService localStorageService;
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
        public CustomAuthStateProvider(HttpClient httpClient, ISyncLocalStorageService localStorageService)
        {
            this._httpClient = httpClient;
            this.localStorageService = localStorageService;

            var accessToken = localStorageService.GetItem<string>("access");
            if (accessToken is not null)
                this._httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            
            if (_currentUser.Identity != null && _currentUser.Identity.IsAuthenticated)
            {
                return new AuthenticationState(_currentUser);
            }

            var token = localStorageService.GetItem<string>("access");

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            
            if (IsTokenExpired(token))
            {
                Logout();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var claims = BuildClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);
           
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            return new AuthenticationState(_currentUser);
        }
        private IEnumerable<Claim> BuildClaimsFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var claims = new List<Claim>();

            foreach (var claim in jwtToken.Claims)
            {
                switch (claim.Type)
                {
                    case "name":
                        claims.Add(new Claim(ClaimTypes.Name, claim.Value));
                        break;
                    case "unique_name":
                        claims.Add(new Claim("UniqueName", claim.Value));
                        break;

                    case "email":
                        claims.Add(new Claim(ClaimTypes.Email, claim.Value));
                        break;

                    case "sub":
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                        break;

                    case "role":
                        claims.Add(new Claim(ClaimTypes.Role, claim.Value));
                        break;
                    case "CardCode":
                        claims.Add(new Claim("CardCode", claim.Value));
                        break;
                    case "UserId":
                        claims.Add(new Claim("UserId", claim.Value));
                        break;
                    default:
                        claims.Add(claim);
                        break;
                }
            }
            // Optional: keep token as claim
            //claims.Add(new Claim("access_token", token));

            return claims;
        }
        public void SetUser(AuthResponse auth)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, auth.Name),
                new Claim(ClaimTypes.Email, auth.Email),
                new Claim("CardCode", auth.CardCode),
                new Claim("UniqueName", auth.Email),
                new Claim("access_token", auth.token),
                new Claim("UserId", auth.UserId)
            };

            var identity = new ClaimsIdentity(claims, "jwt");
            _currentUser = new ClaimsPrincipal(identity);


            localStorageService.SetItem("access", auth.token);
            //localStorageService.SetItem("refreshtoken", auth.token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.token);


            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        private bool IsTokenExpired(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp");

            if (expClaim == null)
                return true;

            var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim.Value));

            return exp < DateTimeOffset.UtcNow;
        }
        public void Logout()
        {
            localStorageService.RemoveItem("access");
            _httpClient.DefaultRequestHeaders.Authorization = null;
          _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
