using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using Portal.Tests.Helpers;
using System;
using System.Linq;
using Xunit;

namespace Portal.Tests
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
        public void ValorTotal_Positive_And_Vendedor_Active_Must_PassValidation()
        {
            var vendedor = new Vendedor("Nome", "529.982.247-25", "email@example.com", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", "10.845.018/0001-48", 150.50m, null, vendedor);

            var results = ValidationHelper.Validate(invoice);

            Assert.Empty(results);
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

        [Theory]
        [InlineData("1234")]
        [InlineData("abcd1234")]
        [InlineData("11111111111")]
        [InlineData("00000000000")]
        [InlineData("123.456.789-01")]
        [InlineData("123.456.789-00")]
        [InlineData("12.345.678/0001-0")] // malformed cnpj
        [InlineData("123456789")] // too short
        [InlineData("12345678901234567890")] // too long
        [InlineData("00.000.000/0000-00")]
        public void InvalidClienteDocumento_Should_FailValidation(string invalidDoc)
        {

            var vendedor = new Vendedor("Nome", "529.982.247-25", "email@example.com", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", invalidDoc, 150.50m, null, vendedor);

            var results = ValidationHelper.Validate(invoice);

            var error = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(Invoice.ClienteDocumento)));

            Assert.NotNull(error);
            Assert.Contains("inválido", error.ErrorMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Invoice_EmptyCliente_Should_FailValidation(string invalidCliente)
        {
            // Arrange
            var vendedor = new Vendedor("Nome", "529.982.247-25", "email@example.com", null, 5m);

            var invoice = new Invoice(vendedor.Id, "Cliente Valido", "529.982.247-25", 100m, null, vendedor);

            // Act (Forçando o estado inválido para testar a DataAnnotation)
            typeof(Invoice).GetProperty(nameof(Invoice.Cliente))?
                .SetValue(invoice, invalidCliente);

            var results = ValidationHelper.Validate(invoice);

            // Assert
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Invoice.Cliente)));
        }

        [Fact]
        public void Invoice_MissingRequiredFields_Should_FailValidation()
        {
            // Arrange: Criamos uma instância sem passar pelo construtor de negócio 
            // para deixar as propriedades com valor default (null ou vazio)
            var invoice = (Invoice)Activator.CreateInstance(typeof(Invoice), true)!;

            // Act
            var results = ValidationHelper.Validate(invoice);

            // Assert
            Assert.NotEmpty(results);

            // 2. Lista de campos que DEVEM gerar erro por estarem nulos/vazios
            var requiredFields = new[]
            {
                nameof(Invoice.Number),
                nameof(Invoice.Cliente),
                nameof(Invoice.ClienteDocumento),
                nameof(Invoice.ValorTotal)
            };

            foreach (var field in requiredFields)
            {
                Assert.Contains(results, r => r.MemberNames.Contains(field));
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void EmptyClienteDocumento_Should_FailValidation(string invalidDoc)
        {

            var vendedor = new Vendedor("Nome", "529.982.247-25", "email@example.com", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", invalidDoc, 150.50m, null, vendedor);

            var results = ValidationHelper.Validate(invoice);

            var error = results.FirstOrDefault(r => r.MemberNames.Contains(nameof(Invoice.ClienteDocumento)));

            Assert.NotNull(error);
            Assert.Contains("required", error.ErrorMessage, StringComparison.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData("52998224725")]
        [InlineData("529.982.247-25")]
        [InlineData("12.345.678/0001-95")]
        public void ValidClienteDocumento_Should_PassValidation(string validDoc)
        {
            var vendedor = new Vendedor("Nome", "529.982.247-25", "email@example.com", null, 5m);
            var invoice = new Invoice(vendedor.Id, "Cliente", validDoc, 100m, null, vendedor);

            var results = ValidationHelper.Validate(invoice);

            Assert.Empty(results);
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