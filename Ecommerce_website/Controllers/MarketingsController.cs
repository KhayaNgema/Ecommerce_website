using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class MarketingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
