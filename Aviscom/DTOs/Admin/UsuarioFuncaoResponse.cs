using NUlid;

namespace Aviscom.DTOs.Admin
{
    public class UsuarioFuncaoResponse
    {
        public Ulid FkUsuarioPfId { get; set; }
        public Ulid FkFuncaoId { get; set; }
        public Ulid FkSetorId { get; set; }

        public string Descricao { get; set; } = string.Empty;

        public string? NomeUsuario { get; set; }
        public string? TituloFuncao { get; set; }
        public string? NomeSetor { get; set; }
    }
}
