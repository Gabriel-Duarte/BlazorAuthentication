namespace BlazorAuthentication.Client.Model
{
    public class UserTokenDto
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? UserName { get; set; } = null!;
    }
}
