using BlazorAuthentication.Client.Model;

namespace BlazorAuthentication.Service.Interfaces
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task LogoutAsync();
    }
}
