using BitzArt.Blazor.Cookies;
using BlazorAuthentication.Client.Model;
using BlazorAuthentication.Client.Service.Interface;
using BlazorAuthentication.Client.Service.Service.Base;
using System.Net.Http.Headers;

namespace BlazorAuthentication.Client.Service.Service
{
    public class RoleService : BaseService, IRoleService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICookieService _cookieService;

        public RoleService(IHttpClientFactory httpClientFactory,
           ICookieService cookieService)
        {
            _httpClientFactory = httpClientFactory;
            _cookieService = cookieService;
        }
        public async Task<ResponseDto<RoleListResponse>> GetListRole()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ApiMobilizeOauth");
                var accessTokenResult = await _cookieService.GetAsync("authToken");
                var accessToken = accessTokenResult.Value;

                var apiUrl = $"/v1/Role/RoleList";

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync(apiUrl);

                var resultRole = await DeserializeObject<ResponseDto<RoleListResponse>>(response);

                if (resultRole is null)
                {
                    return new ResponseDto<RoleListResponse>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Não foi possível processar os dados recebidos do serviço.Verifique se o formato dos dados está correto." }
                    };
                }

                return resultRole;
            }
            catch (Exception ex)
            {
                return new ResponseDto<RoleListResponse>
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Erro durante a execução da solicitação HTTP: {ex.Message}" }
                };
            }
        }
    }
}