using NUlid;

namespace Aviscom.DTOs.Usuario
{
    public class UpdateUsuarioPessoaJuridicaRequest
    {
        public string? RazaoSocial { get; set; }
        public string? NomeFantasia { get; set; }
        public Ulid? FkResponsavelId { get; set; }
    }
}
