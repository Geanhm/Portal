using Microsoft.AspNetCore.Mvc;
using Portal.Application.DTO;
using Portal.Application.Interfaces;
using Portal.Domain.DTO;

namespace Portal.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
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
    }
}
