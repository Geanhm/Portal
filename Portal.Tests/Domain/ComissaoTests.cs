using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System;
using System.Threading;
using Xunit;

namespace Portal.Tests.Domain
{
    public class ComissaoTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithCorrectValuesAndStatusPendente()
        {
            // Arrange
            var invoiceId = Guid.NewGuid();
            var valorInvoice = 1000m;
            var percentual = 10m;

            // Act
            var comissao = new Comissao(valorInvoice, percentual, invoiceId);

            // Assert
            Assert.Equal(valorInvoice, comissao.ValorBase);
            Assert.Equal(percentual, comissao.PercentualAplicado);
            Assert.Equal(100m, comissao.ValorComissao);
            Assert.Equal(ComissaoStatus.Pendente, comissao.Status);
            Assert.Equal(invoiceId, comissao.InvoiceId);
            Assert.True(comissao.DataCalculo <= DateTime.UtcNow);
        }

        [Theory]
        [InlineData(100.00, 10, 10.00)]    // Simples
        [InlineData(100.00, 15, 15.00)]    // Limite superior (15%)
        [InlineData(100.00, 0, 0.00)]      // Limite inferior (0%)
        [InlineData(333.33, 10, 33.33)]    // Arredondamento comum
        [InlineData(150.75, 5.5, 8.29)]    // 150.75 * 0.055 = 8.29125 -> 8.29
        [InlineData(150.75, 5.55, 8.37)]   // 150.75 * 0.0555 = 8.366625 -> 8.37
        public void Calcular_ShouldHandleDifferentValuesAndRounding(decimal valor, decimal percentual, decimal esperado)
        {
            var comissao = new Comissao(valor, percentual, Guid.NewGuid());

            Assert.Equal(esperado, comissao.ValorComissao);
        }

        [Fact]
        public void MarcarPaga_ValidPendingCommission_ShouldUpdateStatusAndDate()
        {
            // Arrange
            var comissao = new Comissao(1000, 10, Guid.NewGuid());
            var dataPagamento = DateTime.UtcNow.AddDays(-1);

            // Act
            comissao.MarcarPaga(dataPagamento);

            // Assert
            Assert.Equal(ComissaoStatus.Paga, comissao.Status);
            Assert.Equal(dataPagamento, comissao.DataPagamento);
        }

        [Fact]
        public void MarcarPaga_WhenAlreadyPaid_ShouldThrowBusinessException()
        {
            // Arrange
            var comissao = new Comissao(1000, 10, Guid.NewGuid());
            comissao.MarcarPaga();

            // Act & Assert
            var ex = Assert.Throws<BusinessException>(() => comissao.MarcarPaga());
            Assert.Equal("Somente comissões pendentes podem ser marcadas como pagas.", ex.Message);
        }

        [Fact]
        public void Cancelar_ShouldUpdateStatusToCancelada()
        {
            // Arrange
            var comissao = new Comissao(1000, 10, Guid.NewGuid());

            // Act
            comissao.Cancelar();

            // Assert
            Assert.Equal(ComissaoStatus.Cancelada, comissao.Status);
        }

        [Fact]
        public void Calcular_ShouldUpdateDataCalculoEveryTime()
        {
            // Arrange
            var comissao = new Comissao(1000, 10, Guid.NewGuid());
            var dataInicial = comissao.DataCalculo;

            Thread.Sleep(1);

            // Act
            comissao.Calcular(2000, 10);

            // Assert
            Assert.True(comissao.DataCalculo > dataInicial);
        }

        [Theory]
        [InlineData(0)]          
        [InlineData(-0.01)] 
        [InlineData(-999.99)]
        public void ValorBase_WhenZeroOrNegative_Should_FailValidation(decimal valorInvalido)
        {
            var invoiceId = Guid.NewGuid();
            var percentualValido = 10m;

            var ex = Assert.Throws<BusinessException>(() =>
                new Comissao(valorInvalido, percentualValido, invoiceId));

            Assert.Equal("Valor base deve ser maior que zero.", ex.Message);
        }

        [Theory]
        [InlineData(15.01)]
        [InlineData(20)]
        [InlineData(-1)]
        public void Calcular_PercentualInvalido_ShouldThrowBusinessException(decimal percentualInvalido)
        {
            // Arrange
            var invoiceId = Guid.NewGuid();

            var ex = Assert.Throws<BusinessException>(() =>
                    new Comissao(1000, percentualInvalido, invoiceId));
            
            Assert.Contains("entre 0 e 15", ex.Message);
        }
    }
}