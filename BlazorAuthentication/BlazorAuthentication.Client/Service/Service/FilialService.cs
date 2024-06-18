using BitzArt.Blazor.Cookies;
using BlazorAuthentication.Client.Model;
using BlazorAuthentication.Client.Service.Interface;
using BlazorAuthentication.Client.Service.Service.Base;
using System.Net.Http.Headers;

namespace BlazorAuthentication.Client.Service.Service
{
    public class FilialService : BaseService, IFilialService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ICookieService _cookieService;

        public FilialService(IHttpClientFactory httpClientFactory,
             ICookieService cookieService)
        {
            _httpClientFactory = httpClientFactory;
            _cookieService = cookieService; ;
        }
        public async Task<ResponseDto<Page<ListFilialResponse>>> GetListFilial(FilialPage filialPage)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ApiMobilizeOauth");
                var accessTokenResult = await _cookieService.GetAsync("authToken");
                var accessToken = accessTokenResult.Value;

                var apiUrl = $"/v1/Filial/FilialList?Page={filialPage.Page}&Size={filialPage.Size}&Sort={filialPage.Sort}&Direction={filialPage.Direction}";

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.GetAsync(apiUrl);

                var resultfilial = await DeserializeObject<ResponseDto<Page<ListFilialResponse>>>(response);

                if (resultfilial is null)
                {
                    return new ResponseDto<Page<ListFilialResponse>>
                    {
                        IsSuccess = false,
                        Errors = new List<string> { "Não foi possível processar os dados recebidos do serviço.Verifique se o formato dos dados está correto." }
                    };
                }

                return resultfilial;
            }
            catch (Exception ex)
            {
                return new ResponseDto<Page<ListFilialResponse>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"Erro durante a execução da solicitação HTTP: {ex.Message}" }
                };
            }
        }
    }
}
