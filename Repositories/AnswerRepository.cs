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

        Task<EditAnswer> GetFromQueueById(int id);
    }

    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<PaginatedResult<AnswerList>> GetAnswerListPage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest)
        {
            string mainSql = @"
                SELECT a.Id, SUBSTRING(a.Text, 1, 50) as Text, a.AnswerStatusId, ast.Name AS StatusName
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

            int id = await connection.QuerySingleAsync<int>(@"
                INSERT INTO Answers
                (AnswerStatusId, CreatedDate, ModifiedDate)
                OUTPUT INSERTED.Id
                VALUES (@AnswerStatusId, @Text, GETDATE(), GETDATE());", answer);

            return id;
        }

        public Task<EditAnswer> GetFromQueueById(int id)
        {
            using var connection = GetConnection();

            return connection.QueryFirstAsync<EditAnswer>($"SELECT * FROM AnswerQueue WHERE Id = @id", new { id });
        }
    }
}
