namespace Portal.Web.ViewModels
{
    public class DashboardViewModels
    {
        public int TotalInvoices { get; set; }
        public int TotalPendentes { get; set; }
        public int TotalAprovadas { get; set; }
        public int TotalCanceladas { get; set; }
        public int TotalCriadasNosUltimos30Dias { get; set; }
        public decimal ValorTotalAprovadas { get; set; }
        public decimal TotalComissoesPendentes { get; set; }
        public decimal TotalComissoesPagas { get; set; }


        public IEnumerable<ListaVendedorViewModels> TopVendedores { get; set; } = Array.Empty<ListaVendedorViewModels>();
        public IEnumerable<ListaVendedorViewModels> ListaVendedores { get; set; } = Array.Empty<ListaVendedorViewModels>();
    }

    public class ListaVendedorViewModels
    {
        public string Nome { get; set; } = null!;
        public decimal TotalComissao { get; set; }
    }
}
