using System.ComponentModel.DataAnnotations;

namespace BlazorAuthentication.Client.Model
{
    public class Role
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O campo Nome deve ter entre {2} e {1} caracteres.", MinimumLength = 3)]
        public string? Name { get; set; } = string.Empty;
    }
}
