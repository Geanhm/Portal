using FluentValidation.TestHelper;
using Portal.Application.DTO;
using Portal.Application.Validators;
using Xunit;

namespace Portal.Tests.ValidatorsTests
{
    public class VendedorUpdateValidatorTests
    {
        private readonly VendedorUpdateValidator _validator = new();

        [Fact]
        public void Update_QuandoCamposForemNulos_DevePassar()
        {
            var dto = new VendedorUpdateDto();

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("123")]
        [InlineData("11111111111")]
        [InlineData("abc")]
        public void Update_QuandoCpfForPreenchidoEInvalido_DeveFalhar(string cpfInvalido)
        {
            var dto = new VendedorUpdateDto { Cpf = cpfInvalido };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.Cpf)
                  .WithErrorMessage("CPF inválido.");
        }

        [Theory]
        [InlineData("email-sem-arroba")]
        [InlineData("user@domain..com")]
        public void Update_QuandoEmailForPreenchidoEInvalido_DeveFalhar(string emailInvalido)
        {
            var dto = new VendedorUpdateDto { Email = emailInvalido };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.Email)
                  .WithErrorMessage("E-mail inválido");
        }

        [Theory]
        [InlineData("abc1234")]
        [InlineData("1234567890123456")]
        public void Update_QuandoTelefoneForPreenchidoEInvalido_DeveFalhar(string telInvalido)
        {
            var dto = new VendedorUpdateDto { Telefone = telInvalido };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.Telefone)
                  .WithErrorMessage("Telefone inválido.");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(15.01)]
        public void Update_QuandoComissaoForPreenchidaEForaDaFaixa_DeveFalhar(decimal comissaoInvalida)
        {
            var dto = new VendedorUpdateDto { PercentualComissao = comissaoInvalida };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.PercentualComissao);
        }

        [Fact]
        public void Update_QuandoDadosForemValidos_DevePassar()
        {
            var dto = new VendedorUpdateDto
            {
                NomeCompleto = "Novo Nome",
                Cpf = "52998224725",
                Email = "novo@email.com",
                Telefone = "11999999999",
                PercentualComissao = 12.5m
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}
