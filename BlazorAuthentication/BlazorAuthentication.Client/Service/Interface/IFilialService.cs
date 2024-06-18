using BlazorAuthentication.Client.Model;

namespace BlazorAuthentication.Client.Service.Interface
{
    public interface IFilialService
    {
        Task<ResponseDto<Page<ListFilialResponse>>> GetListFilial(FilialPage filialPage);
    }
}
