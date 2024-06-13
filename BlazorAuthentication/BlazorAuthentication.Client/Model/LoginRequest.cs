using System.ComponentModel.DataAnnotations;

namespace BlazorAuthentication.Client.Model
{
    public class LoginRequest
    {   [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
