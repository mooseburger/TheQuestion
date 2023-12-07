using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheQuestion.Data.Models;
using TheQuestion.Models.Answer;
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
            var statuses = await _answerRepository.GetAnswerStatuses();

            return Ok(statuses);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> GetPage(AnswerStatusEnum? statusId, SortDirection sortDirection, [FromQuery] PaginatedRequest request)
        {
            var result = await _answerRepository.GetAnswerListPage(statusId, sortDirection, request);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var model = new CreateAnswer();
            var statuses = await _answerRepository.GetAnswerStatuses();
            model.SetStatuses(statuses);
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateAnswer model)
        {
            if (!ModelState.IsValid)
            {
                var statuses = await _answerRepository.GetAnswerStatuses();
                model.SetStatuses(statuses);

                return View(model);
            }

            int id = await _answerRepository.CreateAnswer(model);
            return Redirect($"edit/{id}");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _answerRepository.GetFromQueueById(id);
            var statuses = await _answerRepository.GetAnswerStatuses();
            model.SetStatuses(statuses);

            return View(model);
        }
    }
}
