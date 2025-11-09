using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.Models.Usuario
{
    public class Setor : BaseEntity
    {

        [Required]
        public string Nome { get; set; } = string.Empty;

        public ICollection<UsuarioFuncao> UsuarioFuncoes { get; set; } = new List<UsuarioFuncao>();
    }
}
