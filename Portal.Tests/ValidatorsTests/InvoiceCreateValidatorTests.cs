using FluentValidation.TestHelper;
using Portal.Application.DTO;
using Portal.Application.Validators;
using Xunit;

namespace Portal.Tests.ValidatorsTests
{
    public class InvoiceCreateValidatorTests
    {
        private readonly InvoiceCreateValidator _validator = new();

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
        public void ClienteDocumento_Invalido_DeveTerErroDeValidacao(string docInvalido)
        {
            var dto = new InvoiceCreateDto { ClienteDocumento = docInvalido };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(i => i.ClienteDocumento)
                  .WithErrorMessage("CPF ou CNPJ inválido.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void ClienteDocumento_Vazio_DeveTerErroDeValidacao(string docInvalido)
        {
            var dto = new InvoiceCreateDto { ClienteDocumento = docInvalido };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(i => i.ClienteDocumento)
                  .WithErrorMessage("O documento do cliente é obrigatório.");
        }

        [Fact]
        public void CamposObrigatorios_Vazios_DevemTerErrosDeValidacao()
        {
            var dto = new InvoiceCreateDto();
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(i => i.VendedorId);
            result.ShouldHaveValidationErrorFor(i => i.Cliente);
            result.ShouldHaveValidationErrorFor(i => i.ClienteDocumento);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10.50)]
        public void ValorTotal_NaoPositivo_DeveTerErroDeValidacao(decimal valor)
        {
            var dto = new InvoiceCreateDto { ValorTotal = valor };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(i => i.ValorTotal);
        }
    }
}
