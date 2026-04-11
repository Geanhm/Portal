using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Domain.Entities
{
    public class Vendedor : BaseEntity
    {
        protected Vendedor()
        {
        }

        public Vendedor(string nomeCompleto, string cpf, string email, string? telefone, decimal percentualComissao)
        {
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            Email = email;
            Telefone = telefone;
            PercentualComissao = percentualComissao;
        }

        [Required]
        [MaxLength(200)]
        public string NomeCompleto { get; private set; } = null!;

        [Required]
        [Cpf(ErrorMessage = "CPF inválido.")]
        public string Cpf { get; private set; } = null!;
        
        [Required]
        //[EmailAddress]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "E-mail inválido")]
        [MaxLength(200)]
        public string Email { get; private set; } = null!;
        
        [Phone]
        [MaxLength(15)]
        public string? Telefone { get; private set; }

        [Required]
        [Range(0, 15, ErrorMessage = "Percentual de comissão deve estar entre 0 e 15.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentualComissao { get; private set; }

        [Required]
        public StatusAtivoInativo Status { get; private set; } = StatusAtivoInativo.Ativo;

        public void UpdateVendedor(string nomeCompleto, string cpf, string email, string? telefone, decimal percentualComissao, string? status)
        {
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            Email = email;
            Telefone = telefone;
            PercentualComissao = percentualComissao;
            if (Enum.TryParse<StatusAtivoInativo>(status, true, out var novoStatus))
            {
                Status = novoStatus;
            }
        }
    }
}
