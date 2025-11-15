using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Usuario
{
    public class CreateEmpresaRequest
    {
        [Required]
        [MinLength(3)]
        public string Nome { get; set; } = string.Empty;

        // O CNPJ é opcional, mas se for fornecido,
        // deve estar num formato válido (14 dígitos ou com máscara)
        [RegularExpression(@"^(\d{14}|\d{2}\.\d{3}\.\d{3}\/\d{4}-\d{2})?$",
            ErrorMessage = "O CNPJ deve estar no formato 11222333000144 ou 11.222.333/0001-44")]
        public string? Cnpj { get; set; }
    }
}
