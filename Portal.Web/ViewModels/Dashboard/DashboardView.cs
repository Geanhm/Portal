namespace Portal.Web.ViewModels.Dashboard
{
    public class DashboardView
    {
        public int TotalInvoices { get; set; }
        public int TotalPendentes { get; set; }
        public int TotalAprovadas { get; set; }
        public int TotalCanceladas { get; set; }
        public int TotalCriadasNosUltimos30Dias { get; set; }
        public decimal ValorTotalAprovadas { get; set; }
        public decimal TotalComissoesPendentes { get; set; }
        public decimal TotalComissoesPagas { get; set; }


        public IEnumerable<ListaVendedorView> TopVendedores { get; set; } = Array.Empty<ListaVendedorView>();
        public IEnumerable<ListaVendedorView> ListaVendedores { get; set; } = Array.Empty<ListaVendedorView>();
    }

    public class ListaVendedorView
    {
        public string Nome { get; set; } = null!;
        public decimal TotalComissao { get; set; }
    }

}
