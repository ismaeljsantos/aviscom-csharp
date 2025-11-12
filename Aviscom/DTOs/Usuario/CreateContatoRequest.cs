using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Usuario
{
    public class CreateContatoRequest
    {
        [Required]
        [RegularExpression("^(Email|Celular|Telefone)$", ErrorMessage = "O 'tipo' deve ser 'Email', 'Celular' ou 'Telefone'.")]
        public string Tipo { get; set; } = string.Empty;
        [Required] public string Valor {  get; set; } = string.Empty;
    }
}
