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


//namespace Portal.Web.Controllers
//{
//    // Remova o [ApiController] e o [Route("api/[controller]")] por enquanto
//    public class DashboardController : Controller
//    {
//        private readonly IDashboardService _dashboardService;

//        public DashboardController(IDashboardService dashboardService)
//        {
//            _dashboardService = dashboardService;
//        }

//        [HttpGet]
//        public async Task<IActionResult> Index(DateTime? dataInicio, DateTime? dataFim, Guid? vendedorId, int? statusInvoice)
//        {
//            var filtro = new DashboardFiltroDto
//            {
//                DataInicio = dataInicio,
//                DataFim = dataFim,
//                VendedorId = vendedorId,
//                StatusInvoice = (InvoiceStatus?)statusInvoice
//            };

//            // Busca os dados da camada de aplicação (Services/Repos)
//            var dto = await _dashboardService.GetDashboardAsync(filtro);

//            // Mapeia para a sua View que você criou
//            var viewModel = new DashboardViewModels
//            {
//                TotalInvoices = dto.TotalInvoices,
//                ValorTotalAprovadas = dto.ValorTotalAprovadas,
//                TotalComissoesPendentes = dto.TotalComissoesPendentes,
//                TotalComissoesPagas = dto.TotalComissoesPagas,
//                TopVendedores = dto.TopVendedores.Select(v => new ListaVendedorViewModels
//                {
//                    Nome = v.Nome,
//                    TotalComissao = v.TotalComissao
//                })
//            };

//            return View(viewModel); // Isso vai procurar em Views/Dashboard/Index.cshtml
//        }
//    }
//}
