using System.ComponentModel.DataAnnotations;

namespace Portal.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } //To Do.: Considerar a implementação de um método para atualizar essa propriedade automaticamente.
    }
}
