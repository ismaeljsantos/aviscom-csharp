using System.ComponentModel.DataAnnotations;

namespace Aviscom.Dtos.Auth
{
    public class LoginRequest
    {
        [Required]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        public string Senha { get; set; } = string.Empty;
    }
}