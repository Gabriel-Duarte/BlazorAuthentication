using BlazorAuthentication.Client.Model;

namespace BlazorAuthentication.Client.Service.Interface
{
    public interface IUserService
    {
        Task<ResponseDto<Page<UserProfileResponse>>> GetListUser(UserPage userPage);
        Task<ResponseDto<RegisterUserResponse>> CreateUser(RegisterUserRequest registerUserRequest);
    }
}
