namespace Portal.Web.ViewModels.Vendedores
{
    public class VendedorView
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
