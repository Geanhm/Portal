using Microsoft.AspNetCore.Mvc;

namespace Portal.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DashboardMvcController : Controller
    {
        [HttpGet("/")]
        [HttpGet("Dashboard/Index")]
        [HttpGet("Dashboard")]
        public IActionResult Index() => View();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("Invoices")]
    public class InvoicesMvcController : Controller
    {
        public IActionResult Index() => View();
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("Vendedores")]
    public class VendedoresMvcController : Controller
    {
        public IActionResult Index() => View();
    }
}
