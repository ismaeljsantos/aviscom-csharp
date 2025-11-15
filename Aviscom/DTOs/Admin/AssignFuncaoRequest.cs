using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.DTOs.Admin
{
    public class AssignFuncaoRequest
    {
        [Required]
        public Ulid FkUsuarioPfId { get; set; } // O ID do Usuário PF

        [Required]
        public Ulid FkFuncaoId { get; set; } // O ID da Função (ex: "Administrador")

        [Required]
        public Ulid FkSetorId { get; set; } // O ID do Setor (ex: "Sistema")

        [Required]
        public string Descricao { get; set; } = string.Empty; // Descrição da função
    }
}
