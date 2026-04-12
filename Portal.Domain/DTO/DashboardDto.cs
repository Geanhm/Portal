namespace Portal.Domain.DTO
{
    public class DashboardDto
    {
        public int TotalInvoices { get; set; }
        public int TotalPendentes { get; set; }
        public int TotalAprovadas { get; set; }
        public int TotalCanceladas { get; set; }
        public int TotalCriadasNosUltimos30Dias { get; set; }   
        public decimal ValorTotalAprovadas { get; set; }
        public decimal TotalComissoesPendentes { get; set; }
        public decimal TotalComissoesPagas { get; set; }


        public IEnumerable<ListaVendedorDto> TopVendedores { get; set; } = Array.Empty<ListaVendedorDto>();
        public IEnumerable<ListaVendedorDto> ListaVendedores { get; set; } = Array.Empty<ListaVendedorDto>();
    }

    public class ListaVendedorDto
    {
        public string Nome { get; set; } = null!;
        public decimal TotalComissao { get; set; }
    }
}
