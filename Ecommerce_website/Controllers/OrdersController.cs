﻿using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class OrdersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
