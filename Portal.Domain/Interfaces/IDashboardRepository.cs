using Portal.Domain.DTO;
using Portal.Domain.Entities.Enums;

namespace Portal.Domain.Interfaces
{
    public interface IDashboardRepository
    {
        Task<DashboardDto> GetDashboardConsolidadoAsync
                    (DateTime? inicio, DateTime? fim, 
                    InvoiceStatus? StatusInvoice, Guid? VendedorId, 
                    CancellationToken ct = default);
    }
}
