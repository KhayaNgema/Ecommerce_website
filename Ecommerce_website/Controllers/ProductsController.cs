﻿using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
