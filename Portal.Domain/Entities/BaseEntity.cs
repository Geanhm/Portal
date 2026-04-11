using System.ComponentModel.DataAnnotations;

namespace Portal.Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public Guid Id { get; private set; } = Guid.CreateVersion7();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; } //To Do.: Considerar a implementação de um método para atualizar essa propriedade automaticamente.
    }
}
