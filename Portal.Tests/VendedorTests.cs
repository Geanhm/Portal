using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Tests.Helpers;
using System.Linq;
using Xunit;

namespace Portal.Tests
{
    public class VendedorTests
    {
        [Fact]
        public void ValidVendedor_Should_PassValidation()
        {
            var vendedor = new Vendedor
            {
                NomeCompleto = "Fulano de Tal",
                Cpf = "529.982.247-25",
                Email = "fulano@example.com",
                Telefone = "5511999999999",
                PercentualComissao = 10m,
                Status = StatusAtivoInativo.Ativo
            };

            var results = ValidationHelper.Validate(vendedor);
            Assert.Empty(results);
        }

        [Theory]
        [InlineData("123.456.789-00")]
        [InlineData("123.456.789-00")]
        [InlineData("123.456.789-00")]
        [InlineData("123.456.789-00")]
        [InlineData("123.456.789-00")]
        [InlineData("123.456.789-00")]
        public void InvalidCpf_Should_FailValidation(string cpf)
        {
            var vendedor = new Vendedor
            {
                NomeCompleto = "Fulano",
                Cpf = cpf,
                Email = "fulano@example.com",
                PercentualComissao = 5m
            };

            var results = ValidationHelper.Validate(vendedor);
            Assert.Contains(results, r => r.ErrorMessage?.ToLower().Contains("cpf") == true || r.MemberNames.Contains(nameof(Vendedor.Cpf)));
        }

        [Fact]
        public void PercentualComissao_OutOfRange_Should_FailValidation()
        {
            var vendedor = new Vendedor
            {
                NomeCompleto = "Fulano",
                Cpf = "52998224725",
                Email = "fulano@example.com",
                PercentualComissao = 20m // >15 invalid
            };

            var results = ValidationHelper.Validate(vendedor);
            Assert.Contains(results, r => r.ErrorMessage?.ToLower().Contains("percentual") == true || r.MemberNames.Contains(nameof(Vendedor.PercentualComissao)));
        }
    }
}