using NUlid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviscom.Models.Usuario
{
    public class Escolaridade : BaseEntity
    {
        [Required]
        public string Tipo { get; set; } = string.Empty;
        public string? NomeCurso { get; set; }
        public int? AnoInicio { get; set; }
        public int? AnoConclusao { get; set; }
        public bool Ativo { get; set; } = true;


        public Ulid FkUsuarioId { get; set; }

        [ForeignKey(nameof(FkUsuarioId))]
        public UsuarioPessoaFisica Usuario { get; set; } = null!;

        public Ulid FkInstituicaoId { get; set; }

        [ForeignKey(nameof(FkInstituicaoId))]
        public Instituicao Instituicao { get; set; } = null!;

    }
}
