using BlazorAuthentication.Client.Model;
using BlazorAuthentication.Client.Service.Interface;
using BlazorAuthentication.Client.Service.Service.Base;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using BitzArt.Blazor.Cookies;

namespace BlazorAuthentication.Client.Service.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICookieService _cookieService;

        public UserService(IHttpClientFactory httpClientFactory,
            ICookieService cookieService)
        {
            _httpClientFactory = httpClientFactory;
            _cookieService = cookieService;
        }
        public async Task<ResponseDto<Page<UserProfileResponse>>> GetListUser(UserPage userPage)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ApiMobilizeOauth");
                var accessTokenResult = await _cookieService.GetAsync("authToken");
                var accessToken = accessTokenResult.Value;

                var apiUrl = $"/v1/User/UserProfileList?Page={userPage.Page}&Size={userPage.Size}&Search={userPage.Search}&Sort={userPage.Sort}&Direction={userPage.Direction}&Email={userPage.Email}&Registration={userPage.Registration}&Name={userPage.Name}&Role={userPage.Role}";

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync(apiUrl);

                var resultUser = await DeserializeObject<ResponseDto<Page<UserProfileResponse>>>(response);

                if (resultUser is null)
                {
                    return new ResponseDto<Page<UserProfileResponse>>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Não foi possível processar os dados recebidos do serviço.Verifique se o formato dos dados está correto." }
                    };
                }

                return resultUser;
            }
            catch (Exception ex)
            {
                return new ResponseDto<Page<UserProfileResponse>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Erro durante a execução da solicitação HTTP: {ex.Message}" }
                };
            }
        }

  

    
     
    }
}