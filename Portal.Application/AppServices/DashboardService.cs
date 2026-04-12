using Portal.Application.DTO;
using Portal.Application.Interfaces;
using Portal.Domain.DTO;
using Portal.Domain.Interfaces;

namespace Portal.Application.AppServices
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<DashboardDto> GetDashboardAsync(DashboardFiltroDto filtro, CancellationToken cancellationToken = default)
        {
            var dashboard = await _repository.GetDashboardConsolidadoAsync(filtro.DataInicio, filtro.DataFim, filtro.StatusInvoice, filtro.VendedorId, cancellationToken);
            return dashboard;
        }
    }
}