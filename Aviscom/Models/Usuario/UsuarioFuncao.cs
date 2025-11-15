using NUlid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviscom.Models.Usuario
{
    public class UsuarioFuncao : BaseEntity
    {
        // Chave composta
        public Ulid? FkPessoaFisicaId { get; set; }
        public Ulid? FkPessoaJuridicaId { get; set; }
        public Ulid FkFuncaoId { get; set; }
        public Ulid FkSetorId { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;

        // === Navegação ===
        [ForeignKey(nameof(FkPessoaFisicaId))]
        public UsuarioPessoaFisica? UsuarioFisica { get; set; }

        [ForeignKey(nameof(FkPessoaJuridicaId))]
        public UsuarioPessoaJuridica? UsuarioJuridica { get; set; }

        [ForeignKey(nameof(FkFuncaoId))]
        public Funcao Funcao { get; set; } = null!;

        [ForeignKey(nameof(FkSetorId))]
        public Setor Setor { get; set; } = null!;
    }
}