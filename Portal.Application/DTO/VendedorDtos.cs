using System.ComponentModel.DataAnnotations;

namespace Portal.Application.DTO
{
    public class VendedorCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string NomeCompleto { get; set; } = null!;

        [Required]
        public string Cpf { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = null!;

        public string? Telefone { get; set; }

        [Required]
        [Range(0, 15)]
        public decimal PercentualComissao { get; set; }

        public string? Status { get; set; }
    }

    public class VendedorUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string NomeCompleto { get; set; } = null!;

        [Required]
        public string Cpf { get; set; } = null!;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = null!;

        public string? Telefone { get; set; }

        [Required]
        [Range(0, 15)]
        public decimal PercentualComissao { get; set; }

        public string? Status { get; set; }
    }

    public class VendedorReadDto
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telefone { get; set; }
        public decimal PercentualComissao { get; set; }
        public string Status { get; set; } = null!;
    }
}