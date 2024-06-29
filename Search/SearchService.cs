using Algolia.Search.Clients;
using Microsoft.Extensions.Options;
using TheQuestion.Data.Models;

namespace TheQuestion.Search
{
    public interface ISearchService
    {
        Task IndexAnswer(Answer answer);

        Task BulkIndexAnswers(IEnumerable<Answer> answers);

        Task UpdateAnswerPopularity(int answerId, double rank, int totalVotes);
    }

    public class AnswerIndexRecord
    {
        public AnswerIndexRecord(Answer answer)
        {
            Id = answer.Id;
            Text = answer.Text;
            Rank = answer.Rank;
            TotalVotes = answer.TotalVotes;
            CreatedDate = answer.CreatedDate;
            PublishedDate = answer.PublishedDate;
        }

        public string ObjectID => Id.ToString();
        public int Id { get; set; }
        public string Text { get; set; }
        public double Rank { get; set; }
        public int TotalVotes { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset PublishedDate { get; set; }
    }

    public class SearchService : ISearchService
    {
        private readonly SearchConfiguration _configuration;
        private readonly ISearchClient _client;
        public SearchService(IOptions<SearchConfiguration> options, ISearchClient client) 
        { 
            _configuration = options.Value;
            _client = client;
        }

        public async Task IndexAnswer(Answer answer)
        {
            var index = _client.InitIndex(_configuration.IndexName);
            var response = await index.SaveObjectAsync(new AnswerIndexRecord(answer));
        }

        public async Task BulkIndexAnswers(IEnumerable<Answer> answers)
        {
            var index = _client.InitIndex(_configuration.IndexName);
            var response = await index.SaveObjectsAsync(answers.Select(a => new AnswerIndexRecord(a)));
        }

        public async Task UpdateAnswerPopularity(int answerId, double rank, int totalVotes)
        {
            var index = _client.InitIndex(_configuration.IndexName);
            var response = await index.PartialUpdateObjectAsync(new { ObjectID = answerId.ToString(), Rank = rank, TotalVotes = totalVotes });
        }
    }
}
