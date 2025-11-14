using NUlid; // Adicione este using para o Ulid

namespace Aviscom.Dtos.Auth
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public Ulid UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<string> Funcoes { get; set; } = new List<string>();
    }
}