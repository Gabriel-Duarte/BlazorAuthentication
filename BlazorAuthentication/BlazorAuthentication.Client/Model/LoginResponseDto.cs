namespace BlazorAuthentication.Client.Model
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime ExpiresIn { get; set; }
        public UserTokenDto UserToken { get; set; }
    }
}
