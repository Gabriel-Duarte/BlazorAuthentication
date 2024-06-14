using BlazorAuthentication.Client.Model;
using System.Linq.Dynamic.Core.Tokenizer;

namespace BlazorAuthentication.Client.Service.Authentication
{
    public interface IRefreshTokenService
    {
        Task<ResponseDto<TokenDto>> RefreshToken(TokenDto refreshToken);
    }
}
