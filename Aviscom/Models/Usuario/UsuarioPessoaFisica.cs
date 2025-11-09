using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.Models.Usuario
{
    public class UsuarioPessoaFisica : BaseEntity
    {

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public string CpfCriptografado { get; set; } = string.Empty;
        
        [Required]
        public string CpfHash { get; set; } = string.Empty;
        
        [Required]
        public DateTime DataNascimento { get; set; }

        [Required]
        public string NomeMae { get; set; } = string.Empty;

        public string? NomePai { get; set; }

        [Required]
        public string SenhaHash { get; set; } = string.Empty;

        public DateTime? DataUltimoLogin { get; set; }


        public ICollection<Endereco> Enderecos { get; set; } = new List<Endereco>();
        public ICollection<Contato> Contatos { get; set; } = new List<Contato>();
        public ICollection<Escolaridade> Escolaridades { get; set; } = new List<Escolaridade>();
        public ICollection<ExperienciaProfissional> Experiencias { get; set; } = new List<ExperienciaProfissional>();
        public ICollection<UsuarioFuncao> UsuariosFuncoes { get; set; } = new List<UsuarioFuncao>();

        // === NAVEGAÇÃO: Empresas que este usuário é responsável ===
        public ICollection<UsuarioPessoaJuridica> EmpresasResponsavel { get; set; } = new List<UsuarioPessoaJuridica>();
    }
}
