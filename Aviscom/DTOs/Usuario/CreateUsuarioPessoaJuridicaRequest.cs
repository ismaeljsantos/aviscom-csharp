using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Usuario
{
    public class CreateUsuarioPessoaJuridicaRequest
    {
        [Required]
        public string RazaoSocial { get; set; } = string.Empty;

        public string? NomeFantasia { get; set; }

        [Required]
        [RegularExpression(@"^(\d{14}|\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2})$",
            ErrorMessage = "O CNPJ deve estar no formato 11222333000144 ou 11.222.333/0001-44")]
        public string Cnpj { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        public Ulid FkResponsavelId { get; set; }
    }
}
