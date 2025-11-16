using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Auth
{
    public class LoginPjRequest
    {
        [Required]
        public string Cnpj { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}
