using FluentValidation.TestHelper;
using Portal.Application.DTO;
using Portal.Application.Validators;
using Xunit;

namespace Portal.Tests.ValidatorsTests
{
    public class VendedorCreateValidatorTests
    {
        private readonly VendedorCreateValidator _validator = new();

        [Fact]
        public void ValidVendedor_Should_PassValidation()
        {
            var dto = new VendedorCreateDto
            {

                NomeCompleto = "Nome Valido",
                Cpf = "529.982.247-25",
                Email = "email@example.com",
                Telefone = "5511999999999",
                PercentualComissao = 5m
            };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("11111111111")]
        [InlineData("00000000000")]
        [InlineData("1234567890")]    // 10 digits
        [InlineData("123456789012")]  // 12 digits
        [InlineData("123 456 78901")] // embedded spaces
        [InlineData(" 12345678901 ")] // leading/trailing spaces
        [InlineData("123.456.789-00")]
        [InlineData("123-456-789-00")]
        [InlineData("abcdefghijk")]
        [InlineData("!@#$%^&*()")]
        public void Vendedor_InvalidCpf_ProducesValidationError(string invalidCpf)
        {

            var dto = new VendedorCreateDto { Cpf = invalidCpf };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.Cpf);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(16)]
        public void PercentualComissao_OutOfRange_Should_FailValidation(decimal percentual)
        {
            var dto = new VendedorCreateDto { PercentualComissao = percentual };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.PercentualComissao);
        }

        [Fact]
        public void Vendedor_MissingRequiredFields_ProducesValidationErrors()
        {
            var dto = new VendedorCreateDto();
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.NomeCompleto);
            result.ShouldHaveValidationErrorFor(v => v.Cpf);
            result.ShouldHaveValidationErrorFor(v => v.Email);
        }

        [Theory]
        [InlineData("")]                       // empty
        [InlineData(" ")]                      // whitespace
        [InlineData("plainaddress")]           // missing @
        [InlineData("no-at.domain.com")]       // missing @
        [InlineData("user@.com")]              // domain starts with dot
        [InlineData("user@com")]               // no TLD
        [InlineData("user@domain..com")]       // double dot
        [InlineData("user@domain,com")]        // comma instead of dot
        [InlineData("user@domain@domain.com")] // multiple @
        [InlineData("user name@domain.com")]   // space in local part
        [InlineData("user@domain .com")]       // space in domain
        [InlineData(".user@domain.com")]       // leading dot in local part
        [InlineData("user.@domain.com")]       // trailing dot in local part
        [InlineData("user@-domain.com")]       // invalid hyphen placement
        [InlineData("user@domain#.com")]       // invalid character in domain
        public void Vendedor_InvalidEmail_ProducesValidationError(string invalidEmail)
        {
            var dto = new VendedorCreateDto { Email = invalidEmail };
            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(v => v.Email);
        }

    }
}
