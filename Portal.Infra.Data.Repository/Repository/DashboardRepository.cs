using Microsoft.EntityFrameworkCore;
using Portal.Domain.DTO;
using Portal.Domain.Entities.Enums;
using Portal.Domain.Interfaces;

namespace Portal.Infra.Data.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly PortalDbContext _db;

        public DashboardRepository(PortalDbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<DashboardDto> GetDashboardConsolidadoAsync(DateTime? inicio, DateTime? fim, InvoiceStatus? statusInvoice, Guid? vendedorId, CancellationToken ct = default)
        {
            var query = _db.Invoices.AsNoTracking();

            if (inicio.HasValue) query = query.Where(i => i.DataEmissao >= inicio);
            if (fim.HasValue) query = query.Where(i => i.DataEmissao <= fim);
            if (vendedorId.HasValue) query = query.Where(i => i.VendedorId == vendedorId);
            if (statusInvoice.HasValue) query = query.Where(i => i.Status == statusInvoice);

            var dashboard = new DashboardDto
            {
                TotalInvoices = await query.CountAsync(ct),
                TotalPendentes = await query.CountAsync(i => i.Status == InvoiceStatus.Pendente, ct),
                TotalAprovadas = await query.CountAsync(i => i.Status == InvoiceStatus.Aprovada, ct),
                TotalCanceladas = await query.CountAsync(i => i.Status == InvoiceStatus.Cancelada, ct),

                ValorTotalAprovadas = await query
                    .Where(i => i.Status == InvoiceStatus.Aprovada)
                    .SumAsync(i => i.ValorTotal, ct),

                TotalComissoesPagas = await query
                        .Where(i => i.Comissao != null && i.Comissao!.Status == ComissaoStatus.Paga)  
                        .SumAsync(i => (decimal?)i.Comissao!.ValorComissao, ct) ?? 0m,

                TotalComissoesPendentes = await query
                    .Where(i => i.Comissao != null && i.Comissao.Status == ComissaoStatus.Pendente)
                    .SumAsync(i => (decimal?)i.Comissao!.ValorComissao, ct) ?? 0m,

                ListaVendedores = await query
                    .Where(i => i.Comissao != null)
                    .GroupBy(i => new { i.VendedorId, i.Vendedor.NomeCompleto })
                    .Select(g => new ListaVendedorDto
                    {
                        Nome = g.Key.NomeCompleto,
                        TotalComissao = g.Sum(i => i.Comissao.ValorComissao)
                    })
                    .OrderByDescending(x => x.TotalComissao)
                    .ToListAsync(ct)
            };
            dashboard.TopVendedores = dashboard.ListaVendedores.Take(5);
            dashboard.TotalCriadasNosUltimos30Dias = await _db.Invoices
                .AsNoTracking()
                .CountAsync(i => i.DataEmissao >= DateTime.UtcNow.AddDays(-30), ct);

            return dashboard;
        }
    }
}
