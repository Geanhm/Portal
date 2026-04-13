using Portal.Domain.Entities.Enums;
using System.Text.Json.Serialization;

namespace Portal.Application.DTO
{
    public class VendedorCreateDto
    {
        public string NomeCompleto { get; set; } = null!;
        public string Cpf { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Telefone { get; set; }
        public decimal PercentualComissao { get; set; }
    }

    public class VendedorUpdateDto
    {
        public string? NomeCompleto { get; set; }
        public string? Cpf { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public decimal? PercentualComissao { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StatusAtivoInativo? Status { get; set; }
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