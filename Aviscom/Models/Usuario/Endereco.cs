using NUlid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviscom.Models.Usuario
{
    public class Endereco : BaseEntity
    {

        [Required] public string TipoLogradouro { get; set; } = string.Empty;
        [Required] public string Logradouro { get; set; } = string.Empty;
        [Required] public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        [Required] public string Bairro { get; set; } = string.Empty;
        [Required] public string Cidade { get; set; } = string.Empty;
        [Required] public string Estado { get; set; } = string.Empty;
        [Required] public string Cep { get; set; } = string.Empty;

        public Ulid? FkPessoaFisicaId { get; set; }
        public Ulid? FkPessoaJuridicaId { get; set; }

        [ForeignKey(nameof(FkPessoaFisicaId))]
        public UsuarioPessoaFisica? PessoaFisica { get; set; }

        [ForeignKey(nameof(FkPessoaJuridicaId))]
        public UsuarioPessoaJuridica? PessoaJuridica { get; set; }
    }
}