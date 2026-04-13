using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System;
using Xunit;

namespace Portal.Tests.Domain
{
    public class InvoiceTests
    {
        [Fact]
        public void NewInvoice_Should_CalculateComissaoCorrectly()
        {
            var vendedor = new Vendedor(
                nomeCompleto: "Nome Valido",
                cpf: "529.982.247-25",
                email: "email@example.com",
                telefone: "5511999999999",
                percentualComissao: 10m);
            var valorTotal = 1000m;

            var invoice = new Invoice(vendedor.Id, "Cliente X", "10.845.018/0001-48", valorTotal, "Obs", vendedor);

            Assert.NotNull(invoice.Comissao);
            Assert.Equal(100m, invoice.Comissao.ValorComissao);
            Assert.Equal(InvoiceStatus.Pendente, invoice.Status);
        }

        [Fact]
        public void NewInvoice_WithInactiveVendedor_ShouldThrowBusinessException()
        {
            // Arrange
            var vendedor = new Vendedor("Nome", "529.982.247-25", "email@example.com", null, 5m);
            vendedor.Inativar();

            // Act & Assert
            var ex = Assert.Throws<BusinessException>(() =>
                new Invoice(vendedor.Id, "Cliente", "10.845.018/0001-48", 100m, null, vendedor));

            Assert.Equal("Não é possível criar uma fatura para um vendedor inativo.", ex.Message);
        }


        [Fact]
        public void AtualizarDados_ChangingValue_ShouldRecalculateComissao()
        {
            var v1 = new Vendedor("Vendedor 1", "529.982.247-25", "v1@email.com", null, 10m);
            var v2 = new Vendedor("Vendedor 2", "137.284.810-00", "v2@email.com", null, 5m);
            var invoice = new Invoice(v1.Id, "Cliente", "Doc", 1000m, null, v1);

            invoice.AtualizarDados(null, null, null, null, v2);

            Assert.Equal(50m, invoice.Comissao.ValorComissao);
            Assert.Equal(5m, invoice.Comissao.PercentualAplicado);
            Assert.Equal(v2.Id, invoice.VendedorId);
        }

        [Fact]
        public void AtualizarDados_WithPaidComissao_ShouldThrowException()
        {
            var vendedor = new Vendedor("Vendedor 1", "529.982.247-25", "v1@email.com", null, 10m);
            var invoice = new Invoice(vendedor.Id, "Cliente", "123.456.789-01", 1000m, null, vendedor);
            invoice.Comissao.MarcarPaga();

            var ex = Assert.Throws<BusinessException>(() => invoice.AtualizarDados(null, null, 2000m, null));
            Assert.Contains("paga", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void AprovarInvoice_Should_ChangeStatusAndSetDate()
        {
            var vendedor = new Vendedor("Vendedor 1", "529.982.247-25", "v1@email.com", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", "123.456.789-01", 500m, null, vendedor);

            invoice.AprovarInvoice();

            Assert.Equal(InvoiceStatus.Aprovada, invoice.Status);
            Assert.InRange(invoice.DataEmissao, DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5));
        }

        [Fact]
        public void AprovarInvoice_AlreadyApproved_ShouldThrowException()
        {
            var vendedor = new Vendedor("Nome", "529.982.247-25", "Email", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", "Doc", 500m, null, vendedor);
            invoice.AprovarInvoice(); 

            var exception = Assert.Throws<BusinessException>(() => invoice.AprovarInvoice());
            Assert.Equal("Apenas faturas pendentes podem ser aprovadas.", exception.Message);
        }

        [Fact]
        public void AtualizarDados_CanceledInvoice_ShouldThrowException()
        {
            var vendedor = new Vendedor("Vendedor 1", "529.982.247-25", "v1@email.com", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", "123.456.789-01", 500m, null, vendedor);
            invoice.CancelarInvoice();

            var ex = Assert.Throws<BusinessException>(() => invoice.AtualizarDados("Novo Nome", null, null, null));
            Assert.Contains("cancelada", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void AtualizarDados_ChangingVendedorOrValue_OfApprovedInvoice_ShouldThrowException()
        {
            var vendedor = new Vendedor("Vendedor 1", "529.982.247-25", "v1@email.com", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", "123.456.789-01", 500m, null, vendedor);
            invoice.AprovarInvoice();

            var novoVendedor = new Vendedor("Vendedor 2", "137.284.810-00", "v2@email.com", null, 8m);

            var ex = Assert.Throws<BusinessException>(() => invoice.AtualizarDados(null, null, 600m, null, novoVendedor));
            Assert.Contains("aprovada", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10.50)]
        public void NewInvoice_NonPositiveValue_ShouldThrowBusinessException(decimal total)
        {
            // Arrange
            var vendedor = new Vendedor("Vendedor 1", "529.982.247-25", "v1@email.com", null, 10m);

            // Act & Assert
            var ex = Assert.Throws<BusinessException>(() =>
                new Invoice(vendedor.Id, "Cliente", "52998224725", total, null, vendedor));

            Assert.Contains("maior que zero", ex.Message);
        }


    }
}