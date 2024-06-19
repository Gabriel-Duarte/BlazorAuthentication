using System.ComponentModel.DataAnnotations;

namespace BlazorAuthentication.Client.Model
{
    public class UpdateUserProfileRequest
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string? Id { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido.")]
        public string? Email { get; set; } = string.Empty;
        public string? Name { get; set; } = null!;
        public string? Password { get; set; } = null!;

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Nome deve ter entre {2} e {1} caracteres.", MinimumLength = 3)]
        public string? Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Status é obrigatório.")]
        public bool Status { get; set; }

        public List<Role>? Roles { get; set; }
        public List<Filial>? Filiais { get; set; }
        public string? Registration { get; set; }
    }
}
