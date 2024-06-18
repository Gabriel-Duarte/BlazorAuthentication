using BlazorAuthentication.Client.Model;

namespace BlazorAuthentication.Client.Service.Interface
{
    public interface IRoleService
    {
        Task<ResponseDto<RoleListResponse>> GetListRole();
    }
}
