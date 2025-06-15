using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class InventoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
