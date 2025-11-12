using NUlid;

namespace Aviscom.DTOs.Usuario
{
    public class EnderecoResponse
    {
        public Ulid Id { get; set; }
        public string TipoLogradouro { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento {  get; set; }
        public string Bairro {  get; set; } = string.Empty;
        public string Cidade {  get; set; } = string.Empty;
        public string Estado {  get; set; } = string.Empty;
        public string Cep {  get; set; } = string.Empty;

        public Ulid? FkPessoaFisicaId {  get; set; }
        public Ulid? FkPessoaJuridicaId { get; set; }

    }
}
