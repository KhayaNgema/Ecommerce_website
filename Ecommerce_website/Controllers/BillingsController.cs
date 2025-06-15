using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class BillingsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
