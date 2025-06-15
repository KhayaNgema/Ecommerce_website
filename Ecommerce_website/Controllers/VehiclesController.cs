using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class VehiclesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
