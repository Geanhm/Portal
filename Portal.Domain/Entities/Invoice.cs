using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Domain.Entities
{
    public class Invoice : BaseEntity
    {
        protected Invoice()
        {
        }

        public Invoice(Guid vendedorId, string cliente, string clienteDocumento, decimal valorTotal, string? observacoes, Vendedor vendedor)
        {
            Number = $"INV-{DateTime.UtcNow:yyyyMMddHHmmssfff}-{Guid.NewGuid():N}".ToUpper(); //To do gerar de outra forma mais numerica

            VendedorId = vendedorId;
            Cliente = cliente;
            ClienteDocumento = clienteDocumento;
            ValorTotal = valorTotal;
            Observacoes = observacoes;
            Vendedor = vendedor;    
            Comissao = new Comissao(valorTotal, vendedor.PercentualComissao, Id);
        }

        [Required]
        [MaxLength(100)]
        public string Number { get; private set; } = null!;

        public DateTime DataEmissao { get; private set; }

        [Required]
        public Guid VendedorId { get; private set; }

        [Required]
        [MaxLength(200)]
        public string Cliente { get; private set; } = null!;
        [Required]
        [CpfOrCnpj(ErrorMessage = "CNPJ/CPF do cliente inválido.")]
        public string ClienteDocumento { get; private set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor total deve ser maior que zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; private set; }

        [Required]
        public InvoiceStatus Status { get; private set; } = InvoiceStatus.Pendente;

        [MaxLength(500)]
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
            if (!string.IsNullOrWhiteSpace(cliente)) Cliente = cliente;
            if (!string.IsNullOrWhiteSpace(clienteDocumento)) ClienteDocumento = clienteDocumento;
            if (!string.IsNullOrWhiteSpace(observacoes)) Observacoes = observacoes;

            bool valorMudou = valorTotal.HasValue && valorTotal.Value != ValorTotal;
            bool vendedorMudou = novoVendedor != null;

            if (valorMudou) ValorTotal = valorTotal!.Value;
            if (vendedorMudou) VendedorId = novoVendedor!.Id;

            if (valorMudou || vendedorMudou)
            {
                if (Comissao == null) throw new BusinessException("Erro interno: Comissão não carregada.");

                if ((valorMudou || vendedorMudou) && Comissao?.Status == ComissaoStatus.Paga)
                    throw new BusinessException("Não é possível alterar valores de uma fatura com comissão já paga.");

                decimal percentual = vendedorMudou ? novoVendedor!.PercentualComissao : Comissao.PercentualAplicado;

                Comissao.Calcular(ValorTotal, percentual);
            }
        }
    }
}