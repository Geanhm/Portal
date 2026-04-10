using Portal.Domain.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Domain.Entities
{
    public class Comissao : BaseEntity
    {
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor base deve ser maior que zero.")]
        public decimal ValorBase { get; set; }

        [Required]
        [Range(0, 15, ErrorMessage = "Percentual aplicado deve estar entre 0 e 15.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentualAplicado { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorComissao { get; set; }

        [Required]
        public ComissaoStatus Status { get; set; } = ComissaoStatus.Pendente;

        [Required]
        public DateTime DataCalculo { get; set; }

        public DateTime? DataPagamento { get; set; }

        // Relations
        [Required]
        public Guid InvoiceId { get; set; }
        public Invoice? Invoice { get; set; }

        // Calcula e popula os campos: ValorBase, PercentualAplicado, ValorComissao, DataCalculo e Status
        public void Calcular(decimal valorInvoice, decimal percentualVendedor)
        {
            ValorBase = valorInvoice;
            PercentualAplicado = percentualVendedor;
            ValorComissao = Math.Round(ValorBase * (PercentualAplicado / 100m), 2);
            DataCalculo = DateTime.UtcNow;
            Status = ComissaoStatus.Pendente;
        }

        // Marca como paga e seta DataPagamento
        public void MarcarPaga(DateTime? dataPagamento = null)
        {
            Status = ComissaoStatus.Paga;
            DataPagamento = dataPagamento ?? DateTime.UtcNow;
        }

        public void Cancelar()
        {
            Status = ComissaoStatus.Cancelada;
        }
    }
}