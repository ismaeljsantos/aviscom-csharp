using NUlid;

namespace Aviscom.DTOs.Usuario
{
    public class EscolaridadeResponse
    {
        public Ulid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string? NomeCurso { get; set; }
        public int? AnoInicio { get; set; }
        public int? AnoConclusao { get; set; }
        public bool Ativo { get; set; }

        // IDs
        public Ulid FkUsuarioId { get; set; }
        public Ulid FkInstituicaoId { get; set; }

        // Contexto (Nomes)
        public string? NomeInstituicao { get; set; }
    }
}
