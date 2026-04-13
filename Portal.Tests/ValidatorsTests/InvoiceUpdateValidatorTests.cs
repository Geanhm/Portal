using FluentValidation.TestHelper;
using Portal.Application.DTO;
using Portal.Application.Validators;
using Xunit;

namespace Portal.Tests.ValidatorsTests
{
    public class InvoiceUpdateValidatorTests
    {
        private readonly InvoiceUpdateValidator _validator = new();

        [Fact]
        public void Update_QuandoDocumentoForNulo_DevePassar_PoisNaoEstaSendoAlterado()
        {
            var dto = new InvoiceUpdateDto { ClienteDocumento = null };

            var result = _validator.TestValidate(dto);

            result.ShouldNotHaveValidationErrorFor(i => i.ClienteDocumento);
        }

        [Fact]
        public void Update_QuandoDocumentoForInvalido_DeveFalhar()
        {
            var dto = new InvoiceUpdateDto { ClienteDocumento = "123" };

            var result = _validator.TestValidate(dto);

            result.ShouldHaveValidationErrorFor(i => i.ClienteDocumento)
                  .WithErrorMessage("CPF ou CNPJ inválido.");
        }
    }
}
