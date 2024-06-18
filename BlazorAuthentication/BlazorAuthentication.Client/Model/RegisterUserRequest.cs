using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BlazorAuthentication.Client.Model
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Nome deve ter entre {2} e {1} caracteres.", MinimumLength = 3)]
        public string? Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido.")]
        public string? Email { get; set; } = string.Empty;
        public string? Registration { get; set; }
        public string? Name { get; set; }

        [Required(ErrorMessage = "O campo Senha é obrigatório")]
        public string? Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Roles é obrigatório")]
        public List<Role> Roles { get; set; }

        [Required(ErrorMessage = "O campo Filiais é obrigatório")]
        public List<Filial> Filiais { get; set; } = null!;
    }
}
