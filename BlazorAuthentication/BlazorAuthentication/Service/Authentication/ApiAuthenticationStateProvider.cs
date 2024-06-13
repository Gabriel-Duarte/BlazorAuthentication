using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorAuthentication.Service.Authentication
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookieService _cookieService;
        private readonly NavigationManager _navigation;
        public ApiAuthenticationStateProvider(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, ICookieService cookieService, NavigationManager navigation)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _cookieService = cookieService;
            _navigation = navigation;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //var token = CookieHelper.GetCookie(_httpContextAccessor, "authToken");
            //var username = CookieHelper.GetCookie(_httpContextAccessor, "username");
            var AllCockies = await _cookieService.GetAllAsync();


            var token = AllCockies.Where(x => x.Key == "authToken").Select(x => x.Value).FirstOrDefault();
            var username = AllCockies.Where(x => x.Key == "username").Select(x => x.Value).FirstOrDefault();
            //var username = usernameResult?.Value != null ? Uri.UnescapeDataString(usernameResult.Value) : null;
            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            if (!string.IsNullOrEmpty(username))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
            }

            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("bearer", token);

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public async void MarkUserAsLoggedOut()
        {
            await _cookieService.RemoveAsync("authToken");
            await _cookieService.RemoveAsync("refreshToken");
            await _cookieService.RemoveAsync("expiresIn");
            await _cookieService.RemoveAsync("id");
            await _cookieService.RemoveAsync("email");
            await _cookieService.RemoveAsync("userName");    
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(jwt);
            return jwtToken.Claims.Select(claim =>
            {
                if (claim.Type == "unique_name")
                {
                    return new Claim(ClaimTypes.Name, claim.Value);
                }
                return claim;
            });
        }
    }
}
