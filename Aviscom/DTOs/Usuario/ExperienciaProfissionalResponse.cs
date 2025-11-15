using NUlid;

namespace Aviscom.DTOs.Usuario
{
    public class ExperienciaProfissionalResponse
    {
        public Ulid Id { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public int? AnoEntrada { get; set; }
        public int? AnoSaida { get; set; }
        public string? DescricaoAtividades { get; set; }

        // IDs
        public Ulid FkUsuarioId { get; set; }
        public Ulid FkEmpresaId { get; set; }

        // Contexto (Nomes)
        public string? NomeEmpresa { get; set; }
    }
}
