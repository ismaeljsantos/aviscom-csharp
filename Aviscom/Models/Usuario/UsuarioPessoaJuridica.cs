using NUlid;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Aviscom.Models.Usuario
{
    public class UsuarioPessoaJuridica : BaseEntity
    {
        [Required]
        public string RazaoSocial { get; set; } = string.Empty;

        public string? NomeFantasia { get; set; }

        [Required]
        [StringLength(14)]
        public string Cnpj { get; set; } = string.Empty;

        [Required]
        public string CnpjHash { get; set; } = string.Empty;

        [Required]
        public string SenhaHash { get; set; } = string.Empty;
        
        public DateTime? DataUltimoLogin { get; set; }
       
        public Ulid FkResponsavelId { get; set; }

        // === RESPONSÁVEL ===
        [Required]
        [ForeignKey(nameof(FkResponsavelId))]
        public UsuarioPessoaFisica Responsavel { get; set; } = null!;

        // === NAVEGAÇÃO ===
        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<Contato> Contatos { get; set; } = new List<Contato>();
        public ICollection<ExperienciaProfissional> Experiencias { get; set; } = new List<ExperienciaProfissional>();
        public ICollection<UsuarioFuncao> UsuariosFuncoes { get; set; } = new List<UsuarioFuncao>();
    }
}
