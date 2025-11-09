using NUlid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviscom.Models.Usuario
{
    public class UsuarioFuncao
    {
        // Chave composta
        public Ulid FkUsuarioId { get; set; }
        public Ulid FkFuncaoId { get; set; }
        public Ulid FkSetorId { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;

        // === Pode ser PF ou PJ ===
        [ForeignKey(nameof(FkUsuarioId))]
        public UsuarioPessoaFisica? UsuarioFisica { get; set; }

        [ForeignKey(nameof(FkUsuarioId))]
        public UsuarioPessoaJuridica? UsuarioJuridica { get; set; }

        [ForeignKey(nameof(FkFuncaoId))]
        public Funcao Funcao { get; set; } = null!;

        [ForeignKey(nameof(FkSetorId))]
        public Setor Setor { get; set; } = null!;
    }
}