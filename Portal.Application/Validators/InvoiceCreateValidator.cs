using FluentValidation;
using Portal.Application.DTO;
using Portal.Domain.Validators;

namespace Portal.Application.Validators
{
    public class InvoiceCreateValidator : AbstractValidator<InvoiceCreateDto>
    {
        public InvoiceCreateValidator()
        {
            RuleFor(i => i.VendedorId)
                .NotEmpty().WithMessage("O vendedor é obrigatório.");

            RuleFor(i => i.Cliente)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                .MaximumLength(200);

            RuleFor(i => i.ClienteDocumento)
                .NotEmpty().WithMessage("O documento do cliente é obrigatório.")
                .Must(DocumentValidator.IsCpfOrCnpj).WithMessage("CPF ou CNPJ inválido.");

            RuleFor(i => i.ValorTotal)
                .GreaterThan(0).WithMessage("O valor total deve ser maior que zero.");

            RuleFor(i => i.Observacoes)
                .MaximumLength(500);
        }
    }

    public class InvoiceUpdateValidator : AbstractValidator<InvoiceUpdateDto>
    {
        public InvoiceUpdateValidator()
        {
            RuleFor(i => i.ClienteDocumento)
                .Must(DocumentValidator.IsCpfOrCnpj).WithMessage("CPF ou CNPJ inválido.")
                .When(v => !string.IsNullOrWhiteSpace(v.ClienteDocumento));

            RuleFor(i => i.ValorTotal)
                .NotEmpty().WithMessage("O valor total é obrigatório e deve ser maior que zero.");
        }
    }
}
