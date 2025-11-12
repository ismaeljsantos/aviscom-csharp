using Aviscom.DTOs.Usuario;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Aviscom.Utils.Validators
{
    public class ValidarContatoValorAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var request = (CreateContatoRequest)validationContext.ObjectInstance;

            if (request.Tipo.Equals("Email", StringComparison.OrdinalIgnoreCase))
            {
                var emailValidator = new EmailAddressAttribute();
                if(!emailValidator.IsValid(request.Tipo))
                {
                    return new ValidationResult("O 'valor' fornecido não é um endereço de email válido.");
                }
            }

            if (request.Tipo.Equals("Telefone", StringComparison.OrdinalIgnoreCase) ||
                request.Tipo.Equals("Celular", StringComparison.OrdinalIgnoreCase) )
            {
                string numeroLimpo = Regex.Replace(request.Valor ?? "", @"[^\d]", "");
                if (numeroLimpo.Length > 10 || numeroLimpo.Length > 11)
                {
                    return new ValidationResult("O 'valor' para Telefone/Celular deve conter 10 ou 11 dígitos numéricos, mesmo que formatado.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
