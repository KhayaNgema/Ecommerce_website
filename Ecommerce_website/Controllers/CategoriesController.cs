using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
