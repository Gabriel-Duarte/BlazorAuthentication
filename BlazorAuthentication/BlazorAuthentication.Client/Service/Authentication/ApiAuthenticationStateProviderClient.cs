using BitzArt.Blazor.Cookies;
using BlazorAuthentication.Client.Model;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Globalization;
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
        private readonly IRefreshTokenService _refreshTokenService;
        private SemaphoreSlim refreshTokenSemaphore = new SemaphoreSlim(1, 1);
        public ApiAuthenticationStateProviderClient(
            HttpClient httpClient,
            ICookieService cookieService,
            IRefreshTokenService refreshTokenService)
        {
            _httpClient = httpClient;
            _cookieService = cookieService;
            _refreshTokenService = refreshTokenService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            await refreshTokenSemaphore.WaitAsync();
            try
            {
                var Cockies = await _cookieService.GetAllAsync();
                var savedToken = Cockies.Where(x => x.Key == "authToken").Select(x => x.Value).FirstOrDefault();
                var expirationToken = Cockies.Where(x => x.Key == "expiresIn").Select(x => x.Value).FirstOrDefault();

                if (string.IsNullOrWhiteSpace(savedToken) || TokenExpirou(expirationToken))
                {
                    await RefreshToken();
                }
            }
            finally
            {
                refreshTokenSemaphore.Release();
            }

            var AllCockies = await _cookieService.GetAllAsync();

            var token = AllCockies.Where(x => x.Key == "authToken").Select(x => x.Value).FirstOrDefault();
            var username = AllCockies.Where(x => x.Key == "username").Select(x => x.Value).FirstOrDefault();
            var expiresInString = AllCockies.Where(x => x.Key == "expiresIn").Select(x => x.Value).FirstOrDefault();
          
            if (string.IsNullOrWhiteSpace(token))
            {
                MarkUserAsLoggedOut();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var identity = string.IsNullOrEmpty(token) ? new ClaimsIdentity() : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            if (!string.IsNullOrEmpty(username))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, username));
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
        private bool TokenExpirou(string dataToken)
        {
            DateTime dataAtualUtc = DateTime.UtcNow;
            DateTime dataExpiracao;

            string[] formatos = { "yyyy-MM-dd'T'HH:mm:ss.fffffff'Z'", "yyyy-MM-dd'T'HH:mm:ss'Z'" };

            foreach (var formato in formatos)
            {
                if (DateTime.TryParseExact(dataToken, formato, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dataExpiracao))
                {
                    if (dataExpiracao < dataAtualUtc)
                    {
                        return true;
                    }

                    break;
                }
            }

            return false;
        }

        public async Task RefreshToken()
        {
            try
            {

                TokenDto refreshToken = new TokenDto();
                var AllCockies = await _cookieService.GetAllAsync();

                refreshToken.RefreshToken = AllCockies.Where(x => x.Key == "refreshToken").Select(x => x.Value).FirstOrDefault();
                refreshToken.AccessToken = AllCockies.Where(x => x.Key == "authToken").Select(x => x.Value).FirstOrDefault();


                if (string.IsNullOrWhiteSpace(refreshToken.AccessToken))
                {
                    MarkUserAsLoggedOut();
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                    return;
                }

                var response = await _refreshTokenService.RefreshToken(refreshToken);

                if (response.IsSuccess)
                {
                    var NewexpirationToken = response.Data.ExpiresIn.Value.AddHours(-3).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");
                    DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;

                    await _cookieService.RemoveAsync("authToken");
                    await _cookieService.RemoveAsync("refreshToken");
                    await _cookieService.RemoveAsync("expiresIn");

                    await _cookieService.SetAsync("authToken", response.Data.AccessToken, currentDateTimeOffset.AddMinutes(60));
                    await _cookieService.SetAsync("refreshToken", response.Data.RefreshToken, currentDateTimeOffset.AddMinutes(60));
                    await _cookieService.SetAsync("expiresIn", NewexpirationToken, currentDateTimeOffset.AddMinutes(60));


                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
                }
                else
                {
                    MarkUserAsLoggedOut();
                    _httpClient.DefaultRequestHeaders.Authorization = null;
                }
            }
            catch (Exception ex)
            {
                MarkUserAsLoggedOut();
            }
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

