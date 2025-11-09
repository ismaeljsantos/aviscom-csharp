using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.Models.Usuario
{
    public class Funcao
    {
        [Key]
        public Ulid Id { get; set; } = Ulid.NewUlid();

        [Required]
        public string Titulo { get; set; } = string.Empty;

        public ICollection<UsuarioFuncao> UsuarioFuncoes { get; set; } = new List<UsuarioFuncao>();
    }
}
