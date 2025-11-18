using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Admin
{
    public class AssignFuncaoPjRequest
    {
        [Required]
        public Ulid FkUsuarioPjId { get; set; } // ID do Usuário PJ

        [Required]
        public Ulid FkFuncaoId { get; set; }

        [Required]
        public Ulid FkSetorId { get; set; }

        [Required]
        public string Descricao { get; set; } = string.Empty;
    }
}
