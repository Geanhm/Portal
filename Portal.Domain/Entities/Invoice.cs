using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        public Invoice()
        {
            // generate a readable unique invoice number
            Number = $"INV-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}".ToUpper(); //To do gerar de outra forma mais numerica
            IssueDate = DateTime.UtcNow;
            Status = InvoiceStatus.Pendente;
        }

        [Required]
        [MaxLength(100)]
        public string Number { get; set; } = null!; // unique, generated automatically

        [Required]
        public DateTime IssueDate { get; set; }

        [Required]
        public Guid VendedorId { get; set; }
        public Vendedor? Vendedor { get; set; }

        [Required]
        [MaxLength(200)]
        public string Cliente { get; set; } = null!;

        [Required]
        [CpfOrCnpj(ErrorMessage = "CNPJ/CPF do cliente inválido.")]
        public string ClienteDocumento { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor total deve ser maior que zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [Required]
        public InvoiceStatus Status { get; set; }

        [MaxLength(500)]
        public string? Observacoes { get; set; }



    }
}