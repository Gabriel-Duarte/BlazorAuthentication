using BitzArt.Blazor.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

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
            var AllCockies = await _cookieService.GetAllAsync();


            var token = AllCockies.Where(x => x.Key == "authToken").Select(x => x.Value).FirstOrDefault();
            var username = AllCockies.Where(x => x.Key == "username").Select(x => x.Value).FirstOrDefault();
            var expiresInString = AllCockies.Where(x => x.Key == "expiresIn").Select(x => x.Value).FirstOrDefault();


            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            DateTimeOffset expiresIn;
            bool isTokenExpired = DateTimeOffset.TryParse(expiresInString, out expiresIn) && expiresIn < DateTimeOffset.UtcNow;

            if (isTokenExpired)
            {
                MarkUserAsLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            if (!string.IsNullOrEmpty(username))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
            }

            _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("bearer", token);

            if (string.IsNullOrWhiteSpace(token))
            {
                MarkUserAsLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            return new AuthenticationState(new ClaimsPrincipal(
               new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt")));
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
            var claims = new List<Claim>();

            if (string.IsNullOrEmpty(jwt))
            {
                return claims;
            }

            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);

            try
            {
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

                if (keyValuePairs.TryGetValue("User", out var roles))
                {
                    if (roles is JsonElement rolesJson && rolesJson.ValueKind == JsonValueKind.Array)
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(rolesJson.GetRawText());
                        claims.AddRange(parsedRoles.Select(parsedRole => new Claim("User", parsedRole)));
                    }
                    keyValuePairs.Remove("User");
                }
                else
                {
                    claims.Add(new Claim("User", "DefaultRole"));
                }

                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }

            return claims;
        }
        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}

