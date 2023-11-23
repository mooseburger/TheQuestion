using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TheQuestion.Controllers
{
    public class AnswerController : Controller
    {
        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
