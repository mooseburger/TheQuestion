using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheQuestion.Data.Models;
using TheQuestion.Models.Generic;
using TheQuestion.Repositories;

namespace TheQuestion.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerRepository _answerRepository;

        public AnswerController(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> GetStatuses()
        {
            var statuses = await _answerRepository.GetAnswerStatusFilters();

            return Ok(statuses);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> GetPage(AnswerStatusEnum? statusId, SortDirection sortDirection, [FromQuery] PaginatedRequest request)
        {
            var result = await _answerRepository.GetAnswerListPage(statusId, sortDirection, request);
            return Ok(result);
        }
    }
}
