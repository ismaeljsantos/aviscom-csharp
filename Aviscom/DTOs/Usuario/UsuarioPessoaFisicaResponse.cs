using Aviscom.Utils.JsonConverters;
using NUlid;
using System.Text.Json.Serialization;

namespace Aviscom.DTOs.Usuario
{
    public class UsuarioPessoaFisicaResponse
    {

        public Ulid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? NomeSocial { get; set; }
        public string Sexo { get; set; } = string.Empty;

        [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime DataNascimento { get; set; } 
        public string NomeMae { get; set; } = string.Empty;
        public string? NomePai { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
