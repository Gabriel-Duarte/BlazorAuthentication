using System.Net.Sockets;

namespace BlazorAuthentication.Client.Model
{
    public class UserProfileResponse
    {
        public string Id { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
        public string? UserName { get; set; } = string.Empty;
        public string? Registration { get; set; } = string.Empty;
        public bool Status { get; set; }
        public string? Password { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;

    }
}
