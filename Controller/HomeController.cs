using Microsoft.AspNetCore.Mvc;

namespace BoostifySolution.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}


