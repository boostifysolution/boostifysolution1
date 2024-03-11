using Microsoft.AspNetCore.Mvc;

namespace BoostifySolution.Controllers
{

    [Route("Admin")]
    public class AdminController : Controller
    {
        [HttpGet("SignIn")]
        public IActionResult SignIn()
        {
            return View("AdminSignIn");
        }

        [HttpGet("Users")]
        public IActionResult Users()
        {
            return View("AdminUsers");
        }

        [HttpGet("Tasks")]
        public IActionResult Tasks()
        {
            return View("AdminTasks");
        }

        [HttpGet("AdminStaffs")]
        public IActionResult AdminStaffs()
        {
            return View("AdminStaffs");
        }
    }
}


