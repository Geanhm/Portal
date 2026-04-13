using Portal.Domain.Entities.Enums;
using Portal.Domain.Validators;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Portal.Application.DTO
{
    //VendedorDto generic para os outros herdar, maybe static
    public class VendedorCreateDto
    {
        [Required]
        [MaxLength(200)]
        public string NomeCompleto { get; set; } = null!;

        [Required]
        [Cpf(ErrorMessage = "CPF inválido.")]
        public string Cpf { get; set; } = null!;

        [Required]
        //[EmailAddress]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "E-mail inválido")]
        [MaxLength(200)]
        public string Email { get; set; } = null!;

        //[Phone]
        [RegularExpression(@"^(\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9})?$", ErrorMessage = "Telefone inválido")]
        [MaxLength(15)]
        public string? Telefone { get; set; }

        [Required]
        [Range(0, 15)]
        public decimal PercentualComissao { get; set; }
    }

    public class VendedorUpdateDto
    {
        [MaxLength(200)]
        public string? NomeCompleto { get; set; } = null!;

        [Cpf(ErrorMessage = "CPF inválido.")]
        public string? Cpf { get; set; } = null!;
        
        //[EmailAddress]
        [RegularExpression(@"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$", ErrorMessage = "E-mail inválido")]
        [MaxLength(200)]
        public string? Email { get; set; } = null!;

        //[Phone]
        [RegularExpression(@"^(\+?\d{1,4}?[-.\s]?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,4}[-.\s]?\d{1,9})?$", ErrorMessage = "Telefone inválido")]
        [MaxLength(15)]
        public string? Telefone { get; set; }

        [Range(0, 15)]
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