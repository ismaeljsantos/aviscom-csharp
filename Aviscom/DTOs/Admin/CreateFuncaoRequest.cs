using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Admin
{
    public class CreateFuncaoRequest
    {
        [Required]
        [MinLength(3)]
        public string Titulo { get; set; } = string.Empty;
    }
}
