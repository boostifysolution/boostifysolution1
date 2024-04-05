using Microsoft.AspNetCore.Mvc;

namespace BoostifySolution.Controllers
{

    [Route("Users")]
    public class UsersController : Controller
    {
        [HttpGet("SignIn")]
        public IActionResult SignIn()
        {
            return View("UserSignIn");
        }

        [HttpGet("SignUp")]
        public IActionResult SignUp()
        {
            return View("UserSignUp");
        }

        [HttpGet("Home")]
        public IActionResult Home()
        {
            return View("UserHome");
        }

        [HttpGet("Wallet")]
        public IActionResult Wallet()
        {
            return View("UserWallet");
        }

        [HttpGet("Profile")]
        public IActionResult Profile()
        {
            return View("UserProfile");
        }

        [HttpGet("Shopee")]
        public IActionResult Shopee()
        {
            return View("UserStoreShopee");
        }

        [HttpGet("Lazada")]
        public IActionResult Lazada()
        {
            return View("UserStoreLazada");
        }

        [HttpGet("Amazon")]
        public IActionResult Amazon()
        {
            return View("UserStoreAmazon");
        }
    }
}


