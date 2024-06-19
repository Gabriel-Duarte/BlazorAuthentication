using System.ComponentModel.DataAnnotations;

namespace BlazorAuthentication.Client.Model
{
    public class SelectFilial
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O Name deve ter no máximo {1} caracteres e no mínimo {2}.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O campo Descrição deve ter no máximo {1} caracteres e no mínimo {2}.")]
        public string Description { get; set; }

        public Guid IdCompany { get; set; }
        public bool Select { get; set; }
    }
}
