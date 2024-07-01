using Microsoft.AspNetCore.Mvc;
using TheQuestion.Repositories;
using TheQuestion.Search;

namespace TheQuestion.Controllers
{
    public class AnswerVoteController : ControllerBase
    {
        private readonly IAnswerVoteRepository _answerVoteRepository;
        private readonly ISearchService _searchService;

        public AnswerVoteController(IAnswerVoteRepository answerVoteRepository, ISearchService searchService)
        {
            _answerVoteRepository = answerVoteRepository;
            _searchService = searchService;
        }

        [HttpPost("/answer/vote/{answerId}/up")]
        public async Task<IActionResult> Upvote(int answerId)
        {
            var popularity = await _answerVoteRepository.AddVote(answerId, Request.HttpContext.Connection.RemoteIpAddress!.ToString());

            await _searchService.UpdateAnswerPopularity(answerId, popularity.Item1, popularity.Item2);

            return Ok();
        }

        [HttpPost("/answer/vote/{answerId}/undo")]
        public async Task<IActionResult> UndoVote(int answerId)
        {
            var popularity = await _answerVoteRepository.RemoveVote(answerId, Request.HttpContext.Connection.RemoteIpAddress!.ToString());

            await _searchService.UpdateAnswerPopularity(answerId, popularity.Item1, popularity.Item2);

            return Ok();
        }

        [HttpGet("/answer/vote")]
        public async Task<IActionResult> GetVotes() 
        {
            var upvotedAnswers = await _answerVoteRepository.GetVotesByIpAddress(Request.HttpContext.Connection.RemoteIpAddress!.ToString());

            return Ok(upvotedAnswers);
        }
    }
}
