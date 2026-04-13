using FluentValidation;
using Portal.Application.DTO;
using Portal.Domain.Validators;

namespace Portal.Application.Validators
{
    public class VendedorCreateValidator : AbstractValidator<VendedorCreateDto>
    {
        public VendedorCreateValidator()
        {
            RuleFor(v => v.NomeCompleto)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 200 caracteres.");

            RuleFor(v => v.Cpf)
                .NotEmpty().WithMessage("O CPF é obrigatório.")
                .Must(DocumentValidator.IsCpf).WithMessage("CPF inválido.");

            RuleFor(v => v.Email)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                //.EmailAddress()
                .Matches(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$")
                .WithMessage("E-mail em formato inválido.")
                .MaximumLength(200);

            RuleFor(v => v.Telefone)
                .MaximumLength(15).WithMessage("Telefone muito longo.")
                .Matches(@"^\+?\d+$").WithMessage("Telefone inválido.")
                .When(v => !string.IsNullOrWhiteSpace(v.Telefone));

            RuleFor(v => v.PercentualComissao)
                .InclusiveBetween(0, 15).WithMessage("A comissão deve ser entre 0 e 15%.");
        }
    }

    public class VendedorUpdateValidator : AbstractValidator<VendedorUpdateDto>
    {
        public VendedorUpdateValidator()
        {
            RuleFor(v => v.NomeCompleto)
                .MaximumLength(200)
                .When(v => v.NomeCompleto != null);

            RuleFor(v => v.Cpf)
                .Must(DocumentValidator.IsCpf)
                .WithMessage("CPF inválido.")
                .When(v => !string.IsNullOrWhiteSpace(v.Cpf));

            RuleFor(v => v.Email)
                //.EmailAddress()
                .MaximumLength(200).WithMessage("O e-mail deve ter no máximo 200 caracteres.")
                .Matches(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$")
                .WithMessage("E-mail inválido")
                .When(v => !string.IsNullOrWhiteSpace(v.Email));

            RuleFor(v => v.Telefone)
                .MaximumLength(15).WithMessage("Telefone inválido.")
                .Matches(@"^\+?\d+$").WithMessage("Telefone inválido.")
                .When(v => !string.IsNullOrWhiteSpace(v.Telefone));

            RuleFor(v => v.PercentualComissao)
                .InclusiveBetween(0, 15)
                .When(v => v.PercentualComissao.HasValue);
        }
    }
}
