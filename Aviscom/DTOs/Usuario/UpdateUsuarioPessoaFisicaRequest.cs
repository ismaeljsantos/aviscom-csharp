using Aviscom.Utils.JsonConverters;
using System.Text.Json.Serialization;

namespace Aviscom.DTOs.Usuario
{
    public class UpdateUsuarioPessoaFisicaRequest
    {
        public string? Nome { get; set; }
        public string? NomeSocial { get; set; }
        public string? Sexo { get; set; }

        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime? DataNascimento { get; set; }
        public string? NomeMae { get; set; }
        public string? NomePai { get; set; }
    }
}
