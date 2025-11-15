using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Usuario
{
    public class CreateExperienciaProfissionalRequest
    {
        [Required]
        public string Cargo { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int? AnoEntrada { get; set; }

        [Range(1900, 2100)]
        public int? AnoSaida { get; set; }

        public string? DescricaoAtividades { get; set; }

        [Required]
        public Ulid FkEmpresaId { get; set; } 
    }
}
