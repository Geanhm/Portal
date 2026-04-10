using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Portal.Domain.Validators;

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
        public VendedorStatus Status { get; set; } = VendedorStatus.Ativo;
    }

    public enum VendedorStatus
    {
        Ativo = 1,
        Inativo = 2
    }
}
