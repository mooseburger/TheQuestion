using Dapper;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using TheQuestion.Data.Models;
using TheQuestion.Models.Answer;
using TheQuestion.Models.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace TheQuestion.Repositories
{
    public interface IAnswerRepository
    {
        Task<PaginatedResult<AnswerList>> GetAnswerListPage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest);

        Task<IEnumerable<AnswerStatusDto>> GetAnswerStatuses();
        
        Task<int> CreateAnswer(CreateAnswer answer);

        Task<EditAnswer> GetById(int id);
    }

    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(IConfiguration configuration) : base(configuration) { }

        public Task<PaginatedResult<AnswerList>> GetAnswerListPage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest)
        {
            string mainSql = @"
                SELECT a.Id, a.Title, a.StatusId, ast.Name AS StatusName
                FROM Answers a 
                LEFT JOIN AnswerStatuses ast ON a.StatusId = ast.Id
            ";

            return GetAnswerPage<AnswerList>(mainSql, status, sortDirection, paginatedRequest);
        }

        public async Task<IEnumerable<AnswerStatusDto>> GetAnswerStatuses()
        {
            string sql = @"
                SELECT Id, Name
                FROM AnswerStatuses
            ";

            using var connection = GetConnection();

            var statuses = await connection.QueryAsync<AnswerStatusDto>(sql);
            
            return statuses;
        }

        private async Task<PaginatedResult<T>> GetAnswerPage<T>(string mainSql, AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest) where T : class
        {
            string whereClause = string.Empty;
            if (status.HasValue)
            {
                whereClause = "WHERE StatusId = @status";
            }

            string orderByClause = $"ORDER BY a.Id {(sortDirection == SortDirection.Descending ? "DESC" : "ASC")}";

            using var connection = GetConnection();

            string pageSql = @$"
                {mainSql}
                {whereClause}
                {orderByClause}
                OFFSET @offset ROWS 
                FETCH NEXT @pageSize ROWS ONLY
            ";

            var page = await connection.QueryAsync<T>(pageSql, new
            {
                status,
                offset = paginatedRequest.Offset,
                pageSize = paginatedRequest.PageSize
            });

            string totalResultsSql = $"SELECT COUNT(*) FROM Answers {whereClause}";
            int totalResults = await connection.ExecuteScalarAsync<int>(totalResultsSql, new { status });

            return new PaginatedResult<T>
            {
                Page = page,
                TotalRecords = totalResults
            };
        }

        public async Task<int> CreateAnswer(CreateAnswer model)
        {
            var answer = new Answer()
            {
                StatusId = model.StatusId,
                Title = model.Title,
                Text = model.Text,
                Slug = GenerateSlug(model.Title)
            };

            using var connection = GetConnection();

            await GuaranteeUniqueSlug(connection, answer);

            int id = connection.QuerySingle<int>(@"
                INSERT INTO Answers
                (StatusId, Slug, Title, Text, CreatedDate, ModifiedDate)
                OUTPUT INSERTED.Id
                VALUES (@StatusId, @Slug, @Title, @Text, GETDATE(), GETDATE());", answer);

            return id;
        }

        public Task<EditAnswer> GetById(int id)
        {
            using var connection = GetConnection();

            return connection.QueryFirstAsync<EditAnswer>($"SELECT * FROM Answers WHERE Id = @id", new { id });
        }

        private string GenerateSlug(string phrase)
        {
            string str = RemoveAccent(phrase).ToLower();

            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // invalid chars
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space
            str = str.Substring(0, str.Length <= 40 ? str.Length : 40).Trim(); // cut and trim it
            str = Regex.Replace(str, @"\s", "-"); // hyphens

            return str;
        }

        private string RemoveAccent(string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("UTF-8").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }

        private async Task GuaranteeUniqueSlug(SqlConnection connection, Answer answer)
        {
            var matchingAnswer = await GetBySlug(connection, answer.Slug, null);
            while (matchingAnswer != null)
            {
                answer.Slug += "-" + Guid.NewGuid().ToString().Split('-')[0];
                matchingAnswer = await GetBySlug(connection, answer.Slug, null);
            }
        }

        private Task<Answer> GetBySlug(SqlConnection connection, string slug, AnswerStatusEnum? status)
        {
            string statusClause = string.Empty;
            if (status.HasValue)
            {
                statusClause = "AND StatusId = @status";
            }

            return connection.QueryFirstAsync<Answer>($"SELECT * FROM Answers WHERE Slug = @slug {statusClause}", new { slug, status });
        }
    }
}
