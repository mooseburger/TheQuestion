using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheQuestion.Model.Admin;

namespace TheQuestion.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;

        public AdminController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            return View(new Login());
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, false);
            if (!result.Succeeded)
            {
                login.SignInResult = result;
                return View(login);
            }

            return Redirect("admin");
        }

        [HttpGet("admin")]
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
