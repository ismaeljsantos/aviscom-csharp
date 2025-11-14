using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Admin
{
    public class CreateSetorRequest
    {
        [Required]
        [MinLength(3)]
        public string Nome { get; set; } = string.Empty;
    }
}
