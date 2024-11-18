using Microsoft.AspNetCore.Mvc;

namespace CafeDevCode.Website.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
