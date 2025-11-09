using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.Models.Usuario
{
    public class Empresa : BaseEntity
    {
        [Required]
        public string Nome { get; set; } = string.Empty;
        public string? CNPJ { get; set; }
       
        public ICollection<ExperienciaProfissional> Experiencias { get; set; } = new List<ExperienciaProfissional>();
    }
}
