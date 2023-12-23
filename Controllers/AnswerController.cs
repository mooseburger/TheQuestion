using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheQuestion.Data.Models;
using TheQuestion.Models.Answer;
using TheQuestion.Models.Generic;
using TheQuestion.Repositories;
using TheQuestion.Search;

namespace TheQuestion.Controllers
{
    public class AnswerController : Controller
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly ISearchService _searchService;
        
        public AnswerController(IAnswerRepository answerRepository, ISearchService searchService)
        {
            _answerRepository = answerRepository;
            _searchService = searchService;
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
        public IActionResult Create()
        {
            var model = new CreateAnswer();
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAnswer model)
        {
            if (!ModelState.IsValid)
            {
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
            if (model == null)
            {
                return NotFound();
            }

            var statuses = await _answerRepository.GetAnswerStatuses();
            model.SetStatuses(statuses);

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Reviewer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditAnswer model)
        {
            if (!ModelState.IsValid)
            {
                var statuses = await _answerRepository.GetAnswerStatuses();
                model.SetStatuses(statuses);

                return View(model);
            }

            if (model.Publish)
            {
                var publishedAnswer = await _answerRepository.PublishAnswer(model);
                if (publishedAnswer.Answer != null)
                {
                    await _searchService.IndexAnswer(publishedAnswer.Answer);
                }

                else
                {
                    var statuses = await _answerRepository.GetAnswerStatuses();
                    model.SetStatuses(statuses);
                    model.Errors = new List<string> { publishedAnswer.Error! };
                    return View(model);
                }
            }

            else
            {
                await _answerRepository.EditAnswerInQueue(model);
            }
            
            if (model.Next)
            {
                int nextId = await _answerRepository.GetNextAnswerIdInQueue(model.Id);
                if (nextId > 0)
                {
                    return Redirect($"/answer/edit/{nextId}");
                }
            }

            return Redirect("/answer/dashboard");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            string error = await _answerRepository.DeleteAnswerInQueue(id);

            return Ok(new { succeeded = string.IsNullOrWhiteSpace(error), error });
        }
    }
}
