using Aviscom.Utils.JsonConverters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Aviscom.DTOs.Usuario
{
    public class CreateUsuarioPessoaFisicaRequest
    {
        [Required]
        [MinLength(3)]
        public string Nome { get; set; } = string.Empty;

        public string? NomeSocial { get; set; }

        [Required]
        public string Sexo { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(\d{11}|\d{3}.\d{3}.\d{3}-\d{2})$",
            ErrorMessage ="O CPF deve estar no formato 11122233344 ou 111.222.333-44")]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime DataNascimento { get; set; } 

        [Required]
        public string NomeMae { get; set; } = string.Empty;

        public string? NomePai { get; set; }

        [Required]
        [MinLength(8)]
        public string Senha { get; set; } = string.Empty;

    }
}
