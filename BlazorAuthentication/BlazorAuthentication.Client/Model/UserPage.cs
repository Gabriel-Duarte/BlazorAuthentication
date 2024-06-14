namespace BlazorAuthentication.Client.Model
{
    public class UserPage : Pageable
    {
        public string? Email { get; set; } = null;
        public string? Registration { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Role { get; set; } = null;
    }
}
