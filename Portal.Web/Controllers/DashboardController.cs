using Microsoft.AspNetCore.Mvc;
using Portal.Application.DTO;
using Portal.Application.Interfaces;
using Portal.Domain.DTO;
using Portal.Web.ViewModels.Dashboard;

namespace Portal.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET /api/dashboard?dataInicio=2026-01-01&dataFim=2026-02-01&status=Aprovada&vendedorId=123e4567-e89b-12d3-a456-426614174000
        [HttpGet]
        public async Task<ActionResult<DashboardDto>> Get([FromQuery] DashboardFiltroDto filtro, CancellationToken cancellationToken = default)
        {
            var result = await _dashboardService.GetDashboardAsync(filtro, cancellationToken);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] DashboardFiltroDto filtro, CancellationToken cancellationToken = default)
        {
            // 1. Busca os dados na camada de aplicação
            var dto = await _dashboardService.GetDashboardAsync(filtro, cancellationToken);

            // 2. Mapeia o DTO para a sua ViewModel (DashboardView)
            // Dica: Você pode usar o AutoMapper aqui ou fazer o mapeamento manual
            var viewModel = new DashboardView
            {
                TotalInvoices = dto.TotalInvoices,
                TotalPendentes = dto.TotalPendentes,
                TotalAprovadas = dto.TotalAprovadas,
                TotalCanceladas = dto.TotalCanceladas,
                TotalCriadasNosUltimos30Dias = dto.TotalCriadasNosUltimos30Dias,
                ValorTotalAprovadas = dto.ValorTotalAprovadas,
                TotalComissoesPendentes = dto.TotalComissoesPendentes,
                TotalComissoesPagas = dto.TotalComissoesPagas,
                TopVendedores = dto.TopVendedores.Select(v => new ListaVendedorView
                {
                    Nome = v.Nome,
                    TotalComissao = v.TotalComissao
                }),
                ListaVendedores = dto.ListaVendedores.Select(v => new ListaVendedorView
                {
                    Nome = v.Nome,
                    TotalComissao = v.TotalComissao
                })
            };

            // 3. Retorna a View passando o modelo
            return View(viewModel);
        }
    }
}
