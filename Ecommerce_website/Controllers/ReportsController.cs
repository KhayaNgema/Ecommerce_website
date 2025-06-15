using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
