using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheQuestion.CAPTCHA;
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
        private readonly ICaptchaService _captchaService;
        
        public AnswerController(IAnswerRepository answerRepository, ISearchService searchService, ICaptchaService captchaService)
        {
            _answerRepository = answerRepository;
            _searchService = searchService;
            _captchaService = captchaService;
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
        public async Task<IActionResult> GetQueuePage(AnswerStatusEnum? statusId, SortDirection sortDirection, [FromQuery] PaginatedRequest request)
        {
            var result = await _answerRepository.GetAnswerQueuePage(statusId, sortDirection, request);
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

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public IActionResult Table()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> GetTablePage(SortDirection sortDirection, [FromQuery] PaginatedRequest request)
        {
            var result = await _answerRepository.GetAnswerTablePage(sortDirection, request);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> View(int id)
        {
            var answer = await _answerRepository.GetAnswerView(id);

            return View(answer);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Reviewer")]
        public async Task<IActionResult> Reindex(int id)
        {
            var answer = await _answerRepository.GetAnswerView(id);
            if (answer == null)
            {
                return NotFound();
            }

            await _searchService.IndexAnswer(answer);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BulkReindex()
        {
            int pageSize = 100;
            var page = await _answerRepository.GetAnswerPage(new PaginatedRequest() { PageNumber = 1, PageSize = pageSize });
            int totalPages = (int)Math.Ceiling((decimal)page.TotalRecords / pageSize);
            await _searchService.BulkIndexAnswers(page.Page); // Index first page

            for (int p = 2; p <= totalPages; p++)
            {
                page = await _answerRepository.GetAnswerPage(new PaginatedRequest() { PageNumber = p, PageSize = pageSize });
                await _searchService.BulkIndexAnswers(page.Page);
            }

            return Redirect("/answer/table");
        }

        [HttpGet]
        public IActionResult Submit()
        {
            return View(new SubmitAnswer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SubmitAnswer model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //await _answerRepository.CreateAnswer(model);
            await _captchaService.CaptchaPassed(model.CaptchaToken);
            return Redirect("/thanks");
        }
    }
}
