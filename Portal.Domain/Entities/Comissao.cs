using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Domain.Entities
{
    public class Comissao : BaseEntity
    {
        protected Comissao()
        {
        }

        public Comissao(decimal valorInvoice, decimal percentualVendedor, Guid invoiceId)
        {
            InvoiceId = invoiceId;
            Calcular(valorInvoice, percentualVendedor);
        }
        public decimal ValorBase { get; private set; }
        public decimal PercentualAplicado { get; private set; }
        public decimal ValorComissao { get; private set; }
        public ComissaoStatus Status { get; private set; } = ComissaoStatus.Pendente;
        public DateTime DataCalculo { get; private set; }
        public DateTime? DataPagamento { get; private set; }
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
            if (Status == ComissaoStatus.Paga)
                throw new BusinessException("Não é possível cancelar uma comissão que já foi paga.");
            Status = ComissaoStatus.Cancelada;
        }
    }
}