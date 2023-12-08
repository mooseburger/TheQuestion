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

        Task<EditAnswer?> GetFromQueueById(int id);

        Task<string> PublishAnswer(EditAnswer answer);
        Task EditAnswerInQueue(EditAnswer answer);
        Task<string> DeleteAnswerInQueue(int id);
    }

    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<PaginatedResult<AnswerList>> GetAnswerListPage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest)
        {
            string mainSql = @"
                SELECT a.Id, SUBSTRING(a.Text, 1, 50) as Text, a.AnswerStatusId AS StatusId, ast.Name AS StatusName
                FROM AnswerQueue a 
                LEFT JOIN AnswerStatuses ast ON a.AnswerStatusId = ast.Id
            ";

            string whereClause = string.Empty;
            if (status.HasValue)
            {
                whereClause = "WHERE a.AnswerStatusId = @status";
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

            var page = await connection.QueryAsync<AnswerList>(pageSql, new
            {
                status,
                offset = paginatedRequest.Offset,
                pageSize = paginatedRequest.PageSize
            });

            string totalResultsSql = $"SELECT COUNT(*) FROM AnswerQueue a {whereClause}";
            int totalResults = await connection.ExecuteScalarAsync<int>(totalResultsSql, new { status });

            return new PaginatedResult<AnswerList>
            {
                Page = page,
                TotalRecords = totalResults
            };
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

        public async Task<int> CreateAnswer(CreateAnswer model)
        {
            var answer = new AnswerQueue()
            {
                AnswerStatusId = (int)AnswerStatusEnum.InReview,
                Text = model.Text
            };

            using var connection = GetConnection();

            int id = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO AnswerQueue
                (AnswerStatusId, Text, CreatedDate, ModifiedDate)
                OUTPUT INSERTED.Id
                VALUES (@AnswerStatusId, @Text, GETDATE(), GETDATE());", answer);

            return id;
        }

        public async Task<EditAnswer?> GetFromQueueById(int id)
        {
            using var connection = GetConnection();

            var model = await connection.QueryFirstOrDefaultAsync<EditAnswer>($"SELECT * FROM AnswerQueue WHERE Id = @id", new { id });

            return model;
        }

        public async Task<string> PublishAnswer(EditAnswer answer)
        {
            using var connection = GetConnection();

            var answerInQueue = await connection.QueryFirstOrDefaultAsync<AnswerQueue>($"SELECT * FROM AnswerQueue WHERE Id = @id", new { id = answer.Id });
            if (answerInQueue == null)
            {
                return "Answer not found";
            }

            int newAnswerId = await connection.ExecuteScalarAsync<int>(@"
                INSERT INTO Answers
                (Text, CreatedDate, PublishedDate)
                OUTPUT INSERTED.Id
                VALUES (@Text, @CreatedDate, GETDATE())
            ", answer);

            if (newAnswerId > 0)
            {
                await connection.ExecuteAsync(@"DELETE FROM AnswerQueue WHERE Id = @id", new { id = answer.Id });
            }

            return string.Empty;
        }

        public async Task EditAnswerInQueue(EditAnswer answer)
        {
            using var connection = GetConnection();

            await connection.ExecuteAsync(@"
                UPDATE AnswerQueue
                SET Text = @Text, AnswerStatusId = @AnswerStatusId, ModifiedDate = GETDATE()
                WHERE Id = @Id
            ", answer);
        }

        public async Task<string> DeleteAnswerInQueue(int id)
        {
            using var connection = GetConnection();

            var answer = await connection.QueryFirstOrDefaultAsync<AnswerQueue>($"SELECT * FROM AnswerQueue WHERE Id = @id", new { id });
            if (answer == null)
            {
                return "Answer not found";
            }

            if (answer.AnswerStatusId != (int)AnswerStatusEnum.Rejected)
            {
                return "Only rejected answers can be deleted";
            }

            await connection.ExecuteAsync(@"DELETE FROM AnswerQueue WHERE Id = @id", new { id });

            return string.Empty;
        }
    }
}
