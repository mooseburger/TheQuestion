using Microsoft.AspNetCore.Mvc;

namespace TheQuestion.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
