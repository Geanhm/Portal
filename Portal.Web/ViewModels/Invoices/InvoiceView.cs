namespace Portal.Web.ViewModels.Invoices
{
    public class InvoiceView
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
