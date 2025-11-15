using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Usuario
{
    public class CreateEscolaridadeRequest
    {
        [Required]
        public string Tipo { get; set; } = string.Empty; // Ex: Ensino Médio, Graduação, etc.

        public string? NomeCurso { get; set; }

        [Range(1900, 2100)]
        public int? AnoInicio { get; set; }

        [Range(1900, 2100)]
        public int? AnoConclusao { get; set; }

        public bool Ativo { get; set; } = true;

        [Required]
        public Ulid FkInstituicaoId { get; set; } // O ID da Instituição
    }
}
