using System;
using System.ComponentModel.DataAnnotations;

namespace Portal.Application.DTO
{
    public class InvoiceCreateDto
    {
        [Required]
        public Guid VendedorId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Cliente { get; set; } = null!;

        [Required]
        public string ClienteDocumento { get; set; } = null!;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal ValorTotal { get; set; }

        [MaxLength(500)]
        public string? Observacoes { get; set; }
    }

    public class InvoiceUpdateDto
    {
        public Guid? VendedorId { get; set; }

        [MaxLength(200)]
        public string? Cliente { get; set; }

        public string? ClienteDocumento { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? ValorTotal { get; set; }

        [MaxLength(500)]
        public string? Observacoes { get; set; }
    }

    public class InvoiceReadDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = null!;
        public DateTime DataEmissao { get; set; }
        public Guid VendedorId { get; set; }
        public string Cliente { get; set; } = null!;
        public string ClienteDocumento { get; set; } = null!;
        public decimal ValorTotal { get; set; }
        public string Status { get; set; } = null!;
        public string? Observacoes { get; set; }
    }
}