using NUlid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviscom.Models.Usuario
{
    public class ExperienciaProfissional : BaseEntity
    {
        [Required]
        public string Cargo { get; set; } = string.Empty;
        public int? AnoEntrada { get; set; }
        public int? AnoSaida { get; set; }
        public string? DescricaoAtividades { get; set; }
        
        public Ulid FkUsuarioId { get; set; }
        
        [ForeignKey(nameof(FkUsuarioId))]
        public UsuarioPessoaFisica Usuario { get; set; } = null!;

        public Ulid FkEmpresaId { get; set; }

        [ForeignKey(nameof(FkEmpresaId))]
        public Empresa Empresa { get; set; } = null!;
    }
}
