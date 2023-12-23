using Algolia.Search.Clients;
using Microsoft.Extensions.Options;
using TheQuestion.Data.Models;

namespace TheQuestion.Search
{
    public interface ISearchService
    {
        Task IndexAnswer(Answer answer);
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
            var response = await index.SaveObjectAsync(new { 
                ObjectID = answer.Id.ToString(),
                Id = answer.Id,
                Text = answer.Text,
                CreatedDate = answer.CreatedDate,
                PublishedDate = answer.PublishedDate
            });
        }
    }
}
