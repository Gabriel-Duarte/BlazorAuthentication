
using BitzArt.Blazor.Cookies;
using BlazorAuthentication.Client.Model;
using BlazorAuthentication.Service.Authentication;
using BlazorAuthentication.Service.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BlazorAuthentication.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICookieService _cookieService;
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient, IHttpClientFactory httpClientFactory, AuthenticationStateProvider authenticationStateProvider, IHttpContextAccessor httpContextAccessor, ICookieService cookieService )
        {
            _httpClientFactory = httpClientFactory;
            _authenticationStateProvider = authenticationStateProvider;
            _httpContextAccessor = httpContextAccessor;
            _cookieService = cookieService;
            _httpClient = httpClient;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ApiMobilizeOauth");
                var loginModel = new { Username = username, Password = password };
                var loginResult = await httpClient.PostAsJsonAsync("/v1/Auth/Login", loginModel);

                if (loginResult.IsSuccessStatusCode)
                {
                    var tokenModel = JsonSerializer.Deserialize<ResponseDto<LoginResponseDto>>
                        (await loginResult.Content.ReadAsStringAsync(),
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                    DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;

                    await _cookieService.SetAsync("authToken", tokenModel.Data.AccessToken, currentDateTimeOffset.AddMinutes(60));
                    //await _cookieService.SetAsync("refreshToken", jsonrefreshToken, currentDateTimeOffset.AddMinutes(60));
                    await _cookieService.SetAsync("expiresIn", tokenModel.Data.ExpiresIn.ToString(), currentDateTimeOffset.AddMinutes(60));
                    await _cookieService.SetAsync("id", tokenModel.Data.UserToken.Id, currentDateTimeOffset.AddMinutes(60));
                    await _cookieService.SetAsync("email", tokenModel.Data.UserToken.Email, currentDateTimeOffset.AddMinutes(60));
                    await _cookieService.SetAsync("userName", tokenModel.Data.UserToken.UserName, currentDateTimeOffset.AddMinutes(60));

                    ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(tokenModel.Data.AccessToken);
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", tokenModel.Data.AccessToken);

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Task LogoutAsync()
        {
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            return Task.CompletedTask;
        }
    }

}
