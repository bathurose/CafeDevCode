﻿using Microsoft.AspNetCore.Mvc;

namespace CafeDevCode.Website.Controllers
{
    public class VideoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexPortal()
        {
            return View();
        }
        public IActionResult DetailPortal()
        {
            return View();
        }
    }
}
