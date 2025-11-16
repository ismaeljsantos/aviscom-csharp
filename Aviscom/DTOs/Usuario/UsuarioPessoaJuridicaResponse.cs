using NUlid;

namespace Aviscom.DTOs.Usuario
{
    public class UsuarioPessoaJuridicaResponse
    {
        public Ulid Id { get; set; }
        public string RazaoSocial { get; set; } = string.Empty;
        public string? NomeFantasia { get; set; }
        public string Cnpj { get; set; } = string.Empty; // Retornaremos o CNPJ formatado
        public DateTime DataCriacao { get; set; }

        // Dados do Responsável
        public Ulid FkResponsavelId { get; set; }
        public string? NomeResponsavel { get; set; }
    }
}
