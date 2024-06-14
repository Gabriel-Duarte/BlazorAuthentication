using BlazorAuthentication.Client.Service.Service.Base;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Text.Json;
using System.Text;
using BlazorAuthentication.Client.Model;
using BitzArt.Blazor.Cookies;

namespace BlazorAuthentication.Client.Service.Authentication
{
    public class RefreshTokenService : BaseService, IRefreshTokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICookieService _cookieService;
        public RefreshTokenService(
            IHttpClientFactory httpClientFactory,
            ICookieService cookieService)
        {
            _httpClientFactory = httpClientFactory;
            _cookieService = cookieService;
        }

        public async Task<ResponseDto<TokenDto>> RefreshToken(TokenDto refreshTokens)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ApiMobilizeOauth");

                TokenDto refreshToken = new TokenDto();
                var AllCockies = await _cookieService.GetAllAsync();
                refreshToken.RefreshToken = AllCockies.Where(x => x.Key == "refreshToken").Select(x => x.Value).FirstOrDefault();
                refreshToken.AccessToken = AllCockies.Where(x => x.Key == "authToken").Select(x => x.Value).FirstOrDefault();

                var refreshTokenCreateRequestDtoAsJson = JsonSerializer.Serialize(refreshToken);
                var requestContent = new StringContent(refreshTokenCreateRequestDtoAsJson, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("v1/Auth/Refresh-Token", requestContent);

                var loginResult = await DeserializeObject<ResponseDto<TokenDto>>(response);

                return loginResult;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

    }
}