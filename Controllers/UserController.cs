using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheQuestion.Model.Generic;
using TheQuestion.Repositories;

namespace TheQuestion.Controllers
{
    public class UserController : Controller
    {
        
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPage([FromQuery] PaginatedRequest request)
        {
            var result = await _userRepository.GetUserPage(request);
            return Ok(result);
        }
    }
}
