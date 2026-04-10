using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Tests.Helpers;
using System;
using System.Linq;
using Xunit;

namespace Portal.Tests
{
    public class ComissaoTests
    {
        [Fact]
        public void Calcular_Should_Set_ValuesCorrectly()
        {
            var com = new Comissao();
            decimal valorInvoice = 1000m;
            decimal percentualVendedor = 10m;

            com.Calcular(valorInvoice, percentualVendedor);

            Assert.Equal(valorInvoice, com.ValorBase);
            Assert.Equal(percentualVendedor, com.PercentualAplicado);
            Assert.Equal(Math.Round(valorInvoice * (percentualVendedor / 100m), 2), com.ValorComissao);
            Assert.Equal(ComissaoStatus.Pendente, com.Status);
            Assert.NotEqual(default, com.DataCalculo);
        }

        [Fact]
        public void MarcarPaga_Should_SetStatus_And_DataPagamento()
        {
            var com = new Comissao();
            com.Calcular(500m, 5m);

            com.MarcarPaga();

            Assert.Equal(ComissaoStatus.Paga, com.Status);
            Assert.NotNull(com.DataPagamento);
        }

        [Fact]
        public void Cancelar_Should_SetStatus_Cancelada()
        {
            var com = new Comissao();
            com.Calcular(200m, 5m);

            com.Cancelar();

            Assert.Equal(ComissaoStatus.Cancelada, com.Status);
        }

        [Fact]
        public void ValidationAttributes_Should_EnforceRules()
        {
            var com = new Comissao
            {
                ValorBase = 0m, // invalid
                PercentualAplicado = 20m, // invalid >15
                ValorComissao = 0m,
                DataCalculo = default
            };

            var results = ValidationHelper.Validate(com);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Comissao.ValorBase)) || r.ErrorMessage?.ToLower().Contains("valor base") == true);
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Comissao.PercentualAplicado)) || r.ErrorMessage?.ToLower().Contains("percentual") == true);
        }
    }
}