using NUlid;

namespace Aviscom.DTOs.Usuario
{
    public class InstituicaoResponse
    {
        public Ulid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Cnpj { get; set; }
    }
}
