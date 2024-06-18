namespace BlazorAuthentication.Client.Model
{
    public class RegisterUserResponse
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Registration { get; set; } = null!;
        public string? Name { get; set; } = null!;
        public List<Role> Roles { get; set; } = null!;

    }
}
