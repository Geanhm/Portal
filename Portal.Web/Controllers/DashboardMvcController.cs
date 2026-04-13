using Microsoft.AspNetCore.Mvc;

namespace Portal.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("Dashboard")]
    public class DashboardMvcController : Controller
    {
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
