using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;

namespace Portal.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        protected Invoice()
        {
        }

        public Invoice(Guid vendedorId, string cliente, string clienteDocumento, decimal valorTotal, string? observacoes, Vendedor vendedor)
        {
            if (vendedor.Status == StatusAtivoInativo.Inativo)
                throw new BusinessException("Não é possível criar uma fatura para um vendedor inativo.");

            Number = $"INV-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}".ToUpper();

            VendedorId = vendedorId;
            Cliente = cliente;
            ClienteDocumento = clienteDocumento;
            ValorTotal = valorTotal;
            Observacoes = observacoes;
            Vendedor = vendedor;    
            Comissao = new Comissao(valorTotal, vendedor.PercentualComissao, Id);
        }

        public string Number { get; private set; } = null!;
        public DateTime DataEmissao { get; private set; }
        public Guid VendedorId { get; private set; }
        public string Cliente { get; private set; } = null!;
        public string ClienteDocumento { get; private set; } = null!;
        public decimal ValorTotal { get; private set; }
        public InvoiceStatus Status { get; private set; } = InvoiceStatus.Pendente;
        public string? Observacoes { get; private set; }

        public Comissao Comissao { get; private set; } = null!;
        public Vendedor? Vendedor { get; private set; } = null!;


        public void AprovarInvoice() 
        {             
            if (Status != InvoiceStatus.Pendente)
                throw new BusinessException("Apenas faturas pendentes podem ser aprovadas.");
            Status = InvoiceStatus.Aprovada;
            DataEmissao = DateTime.UtcNow;
        }

        public void CancelarInvoice()
        {
            if (Status == InvoiceStatus.Cancelada)
                throw new BusinessException("A fatura já está cancelada.");
            Status = InvoiceStatus.Cancelada;
            Comissao.Cancelar();
        }

        public void AtualizarDados(string? cliente, string? clienteDocumento, decimal? valorTotal, string? observacoes, Vendedor? novoVendedor = null)
        {
            if(Status == InvoiceStatus.Cancelada)
                throw new BusinessException("Não é possível alterar dados de uma fatura cancelada.");

            if (!string.IsNullOrWhiteSpace(cliente)) Cliente = cliente;
            if (!string.IsNullOrWhiteSpace(clienteDocumento)) ClienteDocumento = clienteDocumento;
            if (!string.IsNullOrWhiteSpace(observacoes)) Observacoes = observacoes;

            bool valorMudou = valorTotal.HasValue && valorTotal.Value != ValorTotal;
            bool vendedorMudou = novoVendedor != null;

            if (valorMudou) ValorTotal = valorTotal!.Value;
            
            if (vendedorMudou)
            {
                VendedorId = novoVendedor!.Id;
                if (Status == InvoiceStatus.Aprovada)
                    throw new BusinessException("Não é possível alterar o vendedor de uma fatura aprovada");

                if (novoVendedor.Status == StatusAtivoInativo.Inativo)
                    throw new BusinessException("Não é possível atualizar a fatura para um vendedor inativo.");
            }

            if (valorMudou || vendedorMudou)
            {
                if (Comissao == null) throw new BusinessException("Erro interno: Comissão não carregada.");

                if (Comissao?.Status == ComissaoStatus.Paga)
                    throw new BusinessException("Não é possível alterar valores de uma fatura com comissão já paga.");

                decimal percentual = vendedorMudou ? novoVendedor!.PercentualComissao : Comissao.PercentualAplicado;

                Comissao.Calcular(ValorTotal, percentual);
            }
        }
    }
}