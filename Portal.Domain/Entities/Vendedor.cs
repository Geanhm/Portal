using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Portal.Domain.Entities
{
    public class Vendedor : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string NomeCompleto { get; set; } = null!;

        [Required]
        [Cpf(ErrorMessage = "CPF inválido.")]
        public string Cpf { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = null!;

        [Phone]
        [MaxLength(15)]
        public string? Telefone { get; set; }

        [Required]
        [Range(0, 15, ErrorMessage = "Percentual de comissão deve estar entre 0 e 15.")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal PercentualComissao { get; set; }

        [Required]
        public StatusAtivoInativo Status { get; set; } = StatusAtivoInativo.Ativo;
    }
}
