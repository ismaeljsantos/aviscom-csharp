using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.Models.Usuario
{
    public class Funcao : BaseEntity
    {

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public ICollection<UsuarioFuncao> UsuarioFuncoes { get; set; } = new List<UsuarioFuncao>();
    }
}
