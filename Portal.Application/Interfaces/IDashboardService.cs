using Portal.Application.DTO;
using Portal.Domain.DTO;

namespace Portal.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardAsync(DashboardFiltroDto filtro, CancellationToken cancellationToken = default);
    }
}
