using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheQuestion.Model.Admin;
using TheQuestion.Model.Generic;
using TheQuestion.Repositories;

namespace TheQuestion.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUserRepository _userRepository;

        public AdminController(SignInManager<IdentityUser> signInManager, IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new Login());
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, true);
            if (!result.Succeeded)
            {
                login.SignInResult = result;
                return View(login);
            }

            return Redirect("/admin");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users([FromQuery] PaginatedRequest request)
        {
            var result = await _userRepository.GetUserPage(request);
            return Ok(result);
        }
    }
}
