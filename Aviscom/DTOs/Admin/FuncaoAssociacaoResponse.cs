using NUlid;

namespace Aviscom.DTOs.Admin
{
    public class FuncaoAssociacaoResponse
    {
        public Ulid UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string TipoUsuario { get; set; } = string.Empty;
        public string DescricaoFuncao { get; set; } = string.Empty;
        public string NomeSetor { get; set; } = string.Empty;
    }
}
