using NUlid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviscom.Models.Usuario
{
    public class Contato : BaseEntity
    {
        [Required] public string Tipo { get; set; } = string.Empty; // Email, Telefone, etc.
        [Required] public string Valor { get; set; } = string.Empty;

  

        // === FKs para PF e PJ ===
        public Ulid? FkPessoaFisicaId { get; set; }
        public Ulid? FkPessoaJuridicaId { get; set; }

        [ForeignKey(nameof(FkPessoaFisicaId))]
        public UsuarioPessoaFisica? PessoaFisica { get; set; }

        [ForeignKey(nameof(FkPessoaJuridicaId))]
        public UsuarioPessoaJuridica? PessoaJuridica { get; set; }
    }
}