using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace BlazorAuthentication.Client.Service.Authentication
{
    public class ApiAuthenticationStateProviderClient : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ICookieService _cookieService;

        public ApiAuthenticationStateProviderClient(HttpClient httpClient, ICookieService cookieService)
        {
            _httpClient = httpClient;
            _cookieService = cookieService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var tokenResult = await _cookieService.GetAsync("authToken");
            var usernameResult = await _cookieService.GetAsync("username");

            var token = tokenResult?.Value != null ? Uri.UnescapeDataString(tokenResult.Value) : null;
            var username = usernameResult?.Value != null ? Uri.UnescapeDataString(usernameResult.Value) : null;

            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            if (!string.IsNullOrEmpty(username))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
            }

            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("bearer", token);

            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
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
