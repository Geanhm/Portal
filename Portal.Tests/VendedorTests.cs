using Portal.Domain.Entities;
using Portal.Domain.Entities.Enums;
using Portal.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Portal.Tests
{
    public class VendedorTests
    {
        [Fact]
        public void ValidVendedor_Should_PassValidation()
        {
            var vendedor = new Vendedor(
                nomeCompleto: "Nome Valido",
                cpf: "529.982.247-25",
                email: "email@example.com",
                telefone: "5511999999999",
                percentualComissao: 5m);

            var results = ValidationHelper.Validate(vendedor);
            Assert.Equal(StatusAtivoInativo.Ativo, vendedor.Status);
            Assert.Empty(results);
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
            var vendedor = new Vendedor(
                nomeCompleto: "Nome Valido",
                cpf: invalidCpf,
                email: "email@example.com",
                telefone: null,
                percentualComissao: 5m);

            var results = ValidationHelper.Validate(vendedor);

            Assert.NotEmpty(results);

            var hasCpfError = results.Any(r =>
                r.MemberNames.Contains(nameof(Vendedor.Cpf)) ||
                (r.ErrorMessage?.Contains("CPF", StringComparison.OrdinalIgnoreCase) ?? false));

            Assert.True(hasCpfError,
                $"Expected validation error for CPF when value is '{invalidCpf}'. Actual results: {string.Join(" | ", results.Select(r => r.ErrorMessage))}");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(16)]
        public void PercentualComissao_OutOfRange_Should_FailValidation(decimal percentual)
        {
            var vendedor = new Vendedor(
                nomeCompleto: "Nome Valido",
                cpf: "529.982.247-25",
                email: "email@example.com",
                telefone: null,
                percentualComissao: percentual);

            var results = ValidationHelper.Validate(vendedor);
            Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(Vendedor.PercentualComissao)));
        }

        [Fact]
        public void Vendedor_MissingRequiredFields_ProducesValidationErrors()
        {
            var vendedor = new Vendedor(
                nomeCompleto: string.Empty,
                cpf: string.Empty,
                email: string.Empty,
                telefone: null,
                percentualComissao: 0m);

            var results = ValidationHelper.Validate(vendedor);

            Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(Vendedor.NomeCompleto)));
            Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(Vendedor.Cpf)));
            Assert.Contains(results, r => r.MemberNames.Any(m => m == nameof(Vendedor.Email)));
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
            var results = ValidateEmailProperty(invalidEmail);


            var hasError = results.Any(r =>
                r.MemberNames.Contains(nameof(Vendedor.Email)) ||
                (r.ErrorMessage?.Contains("Email", StringComparison.OrdinalIgnoreCase) ?? false)
            );

            Assert.True(hasError,
                $"Expected validation error for Email when value is: '{invalidEmail}'");

            // Verifica se o erro está associado ao campo Email
            Assert.Contains(results, r => r.MemberNames.Contains(nameof(Vendedor.Email)));
        }

        [Fact]
        public void Vendedor_EmptyEmail_ProducesRequiredValidationError()
        {
            var results = ValidateEmailProperty(string.Empty);

            Assert.NotEmpty(results);
            Assert.All(results, r => Assert.Contains(nameof(Vendedor.Email), r.MemberNames));
        }

        private static IList<ValidationResult> ValidateEmailProperty(string email)
        {
            var vendedor = new Vendedor(
                nomeCompleto: "Nome Valido",
                cpf: "12345678909",
                email: email,
                telefone: null,
                percentualComissao: 5m);

            var results = new List<ValidationResult>();
            var context = new ValidationContext(vendedor) { MemberName = nameof(Vendedor.Email) };
            Validator.TryValidateProperty(vendedor.Email, context, results);
            return results;
        }
    }
}
