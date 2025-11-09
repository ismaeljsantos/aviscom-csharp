using NUlid;
using System.ComponentModel.DataAnnotations;

namespace Aviscom.Models
{
    public abstract class BaseEntity
    {
        [Key]
        public Ulid Id { get; set; } = Ulid.NewUlid();

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime DataAtualizacao { get; set; } = DateTime.UtcNow;
    }
}
