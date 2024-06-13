using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorAuthentication.Service.Authentication
{
    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(jwt);
            return jwtToken.Claims;
        }
    }
}
