using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.Models.Usuario
{
    public class Instituicao : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? CNPJ { get; set; }
       
        public ICollection<Escolaridade> EscolaridadesAssociadas { get; set; } = new List<Escolaridade>();
    }
}
