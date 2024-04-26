using Dapper;
using TheQuestion.Data.Models;
using TheQuestion.Models.Answer;
using TheQuestion.Models.Generic;

namespace TheQuestion.Repositories
{
    public interface IAnswerRepository
    {
        Task<PublicAnswer> GetPublicAnswer(int id);

        Task<PaginatedResult<AnswerQueueTable>> GetAnswerQueuePage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest);

        Task<IEnumerable<AnswerStatusDto>> GetAnswerStatuses();
        
        Task<int> CreateAnswer(string answerText);

        Task<EditAnswer?> GetFromQueueById(int id);

        Task<PublishedAnswer> PublishAnswer(EditAnswer answer);

        Task EditAnswerInQueue(EditAnswer answer);

        Task<string> DeleteAnswerInQueue(int id);

        Task<int> GetNextAnswerIdInQueue(int id);

        Task<PaginatedResult<AnswerTable>> GetAnswerTablePage(SortDirection sortDirection, PaginatedRequest paginatedRequest);

        Task<ViewAnswer> GetAnswerView(int id);

        Task<PaginatedResult<Answer>> GetAnswerPage(PaginatedRequest paginatedRequest);
    }

    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<PublicAnswer> GetPublicAnswer(int id)
        {
            string sql = @"SELECT *, (SELECT MAX(""Id"") FROM ""Answers"") AS LastId FROM ""Answers"" WHERE ""Id"" = @id";

            using var connection = GetConnection();

            var answer = await connection.QueryFirstOrDefaultAsync<PublicAnswer>(sql, new { id });

            return answer;
        }

        public async Task<PaginatedResult<AnswerQueueTable>> GetAnswerQueuePage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest)
        {
            string mainSql = @"
                SELECT a.""Id"", SUBSTRING(a.""Text"", 1, 50) as Text, a.""AnswerStatusId"" AS StatusId, ast.""Name"" AS StatusName
                FROM ""AnswerQueue"" a 
                LEFT JOIN ""AnswerStatuses"" ast ON a.""AnswerStatusId"" = ast.""Id""
            ";

            string whereClause = string.Empty;
            if (status.HasValue)
            {
                whereClause = @"WHERE a.""AnswerStatusId"" = @status";
            }

            string orderByClause = @$"ORDER BY a.""Id"" {(sortDirection == SortDirection.Descending ? "DESC" : "ASC")}";

            using var connection = GetConnection();

            string pageSql = @$"
                {mainSql}
                {whereClause}
                {orderByClause}
                LIMIT @pageSize OFFSET @offset
            ";

            var page = await connection.QueryAsync<AnswerQueueTable>(pageSql, new
            {
                status,
                offset = paginatedRequest.Offset,
                pageSize = paginatedRequest.PageSize
            });

            string totalResultsSql = @$"SELECT COUNT(*) FROM ""AnswerQueue"" a {whereClause}";
            int totalResults = await connection.ExecuteScalarAsync<int>(totalResultsSql, new { status });

            return new PaginatedResult<AnswerQueueTable>
            {
                Page = page,
                TotalRecords = totalResults
            };
        }

        public async Task<IEnumerable<AnswerStatusDto>> GetAnswerStatuses()
        {
            string sql = @"
                SELECT ""Id"", ""Name""
                FROM ""AnswerStatuses""
            ";

            using var connection = GetConnection();

            var statuses = await connection.QueryAsync<AnswerStatusDto>(sql);
            
            return statuses;
        }

        public async Task<int> CreateAnswer(string answerText)
        {
            var answer = new AnswerQueue()
            {
                AnswerStatusId = (int)AnswerStatusEnum.InReview,
                Text = answerText
            };

            using var connection = GetConnection();

            int id = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO ""AnswerQueue""
                (""AnswerStatusId"", ""Text"", ""CreatedDate"", ""ModifiedDate"")
                VALUES (@AnswerStatusId, @Text, NOW(), NOW())
                RETURNING ""Id""
                ", answer);

            return id;
        }

        public async Task<EditAnswer?> GetFromQueueById(int id)
        {
            using var connection = GetConnection();

            var model = await connection.QueryFirstOrDefaultAsync<EditAnswer>($@"SELECT * FROM ""AnswerQueue"" WHERE ""Id"" = @id", new { id });

            return model;
        }

        public async Task<PublishedAnswer> PublishAnswer(EditAnswer editAnswer)
        {
            using var connection = GetConnection();

            var answerInQueue = await connection.QueryFirstOrDefaultAsync<AnswerQueue>($@"SELECT * FROM ""AnswerQueue"" WHERE ""Id"" = @id", new { id = editAnswer.Id });
            if (answerInQueue == null)
            {
                return new PublishedAnswer() { Error = "Answer not found" };
            }

            var answer = new Answer()
            {
                Text = editAnswer.Text,
                CreatedDate = editAnswer.CreatedDate,
                PublishedDate = DateTimeOffset.UtcNow
            };

            answer.Id = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO ""Answers""
                (""Text"", ""CreatedDate"", ""PublishedDate"")
                VALUES (@Text, @CreatedDate, @PublishedDate)
                RETURNING ""Id""
            ", answer);

            if (answer.Id > 0)
            {
                await connection.ExecuteAsync(@"DELETE FROM ""AnswerQueue"" WHERE ""Id"" = @id", new { id = editAnswer.Id });
                return new PublishedAnswer() { Answer = answer };
            }

            return new PublishedAnswer() { Error = "Failed to insert answer" };
        }

        public async Task EditAnswerInQueue(EditAnswer answer)
        {
            using var connection = GetConnection();

            await connection.ExecuteAsync(@"
                UPDATE ""AnswerQueue""
                SET ""Text"" = @Text, ""AnswerStatusId"" = @AnswerStatusId, ""ModifiedDate"" = NOW()
                WHERE ""Id"" = @Id
            ", answer);
        }

        public async Task<string> DeleteAnswerInQueue(int id)
        {
            using var connection = GetConnection();

            var answer = await connection.QueryFirstOrDefaultAsync<AnswerQueue>(@$"SELECT * FROM ""AnswerQueue"" WHERE ""Id"" = @id", new { id });
            if (answer == null)
            {
                return "Answer not found";
            }

            if (answer.AnswerStatusId != (int)AnswerStatusEnum.Rejected)
            {
                return "Only rejected answers can be deleted";
            }

            await connection.ExecuteAsync(@"DELETE FROM ""AnswerQueue"" WHERE ""Id"" = @id", new { id });

            return string.Empty;
        }

        public async Task<int> GetNextAnswerIdInQueue(int id)
        {
            using var connection = GetConnection();

            int nextId = await connection.ExecuteScalarAsync<int>(@"
                SELECT TOP 1 ""Id""
                FROM ""AnswerQueue""
                WHERE ""Id"" > @sourceId AND ""AnswerStatusId"" = @inReviewId
                ORDER BY ""Id"" ASC
            ", new { sourceId = id, inReviewId = AnswerStatusEnum.InReview });

            return nextId;
        }

        public async Task<PaginatedResult<AnswerTable>> GetAnswerTablePage(SortDirection sortDirection, PaginatedRequest paginatedRequest)
        {
            string mainSql = @"
                SELECT ""Id"", SUBSTRING(""Text"", 1, 50) as Text
                FROM ""Answers""
            ";

            string orderByClause = $@"ORDER BY ""Id"" {(sortDirection == SortDirection.Descending ? "DESC" : "ASC")}";

            using var connection = GetConnection();

            string pageSql = @$"
                {mainSql}
                {orderByClause}
                LIMIT @pageSize OFFSET @offset
            ";

            var page = await connection.QueryAsync<AnswerTable>(pageSql, new
            {
                offset = paginatedRequest.Offset,
                pageSize = paginatedRequest.PageSize
            });

            string totalResultsSql = @$"SELECT COUNT(*) FROM ""Answers""";
            int totalResults = await connection.ExecuteScalarAsync<int>(totalResultsSql);

            return new PaginatedResult<AnswerTable>
            {
                Page = page,
                TotalRecords = totalResults
            };
        }

        public async Task<ViewAnswer> GetAnswerView(int id)
        {
            using var connection = GetConnection();

            var answer = await connection.QueryFirstAsync<ViewAnswer>(@"SELECT * FROM ""Answers"" WHERE ""Id"" = @id", new { id });

            return answer;
        }

        public async Task<PaginatedResult<Answer>> GetAnswerPage(PaginatedRequest paginatedRequest)
        {
            string mainSql = @"
                SELECT *
                FROM ""Answers""
            ";

            string orderByClause = @"ORDER BY ""Id"" DESC";

            using var connection = GetConnection();

            string pageSql = @$"
                {mainSql}
                {orderByClause}
                LIMIT @pageSize OFFSET @offset
            ";

            var page = await connection.QueryAsync<Answer>(pageSql, new
            {
                offset = paginatedRequest.Offset,
                pageSize = paginatedRequest.PageSize
            });

            string totalResultsSql = @$"SELECT COUNT(*) FROM ""Answers""";
            int totalResults = await connection.ExecuteScalarAsync<int>(totalResultsSql);

            return new PaginatedResult<Answer>
            {
                Page = page,
                TotalRecords = totalResults
            };
        }
    }
}
