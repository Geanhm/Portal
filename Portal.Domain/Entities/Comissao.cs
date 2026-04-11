using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Domain.Entities
{
    public class Comissao : BaseEntity
    {
        public Comissao()
        {
        }

        public Comissao(decimal valorInvoice, decimal percentualVendedor, Guid invoiceId)
        {
            InvoiceId = invoiceId;
            Calcular(valorInvoice, percentualVendedor);
        }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor base deve ser maior que zero.")]
        public decimal ValorBase { get; private set; }

        [Required]
        [Range(typeof(decimal), "0", "15", ErrorMessage = "Percentual aplicado deve estar entre 0 e 15.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentualAplicado { get; private set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorComissao { get; private set; } = 0;
        
        [Required]
        public ComissaoStatus Status { get; private set; } = ComissaoStatus.Pendente;

        [Required]
        public DateTime DataCalculo { get; private set; }

        public DateTime? DataPagamento { get; private set; }

        [Required]
        public Guid InvoiceId { get; private set; }

        public Invoice? Invoice { get; private set; }

        public void Calcular(decimal valorInvoice, decimal percentualVendedor)
        {
            if (percentualVendedor < 0 || percentualVendedor > 15)
                throw new BusinessException("Percentual aplicado deve estar entre 0 e 15.");

            if (valorInvoice <= 0)
                throw new BusinessException("Valor base deve ser maior que zero.");

            ValorBase = valorInvoice;
            PercentualAplicado = percentualVendedor;
            ValorComissao = Math.Round(ValorBase * (PercentualAplicado / 100m), 2);
            DataCalculo = DateTime.UtcNow;
        }

        public void MarcarPaga(DateTime? dataPagamento = null)
        {
            if(Status != ComissaoStatus.Pendente)
                throw new BusinessException("Somente comissões pendentes podem ser marcadas como pagas.");
            Status = ComissaoStatus.Paga;
            DataPagamento = dataPagamento ?? DateTime.UtcNow;
        }

        public void Cancelar()
        {
            Status = ComissaoStatus.Cancelada;
        }
    }
}