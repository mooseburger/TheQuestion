using Dapper;
using TheQuestion.Data.Models;
using TheQuestion.Models.Answer;
using TheQuestion.Models.Generic;

namespace TheQuestion.Repositories
{
    public interface IAnswerRepository
    {
        Task<PaginatedResult<AnswerList>> GetAnswerListPage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest);

        Task<IEnumerable<AnswerStatusFilter>> GetAnswerStatusFilters();
    }

    public class AnswerRepository : BaseRepository, IAnswerRepository
    {
        public AnswerRepository(IConfiguration configuration) : base(configuration) { }

        public Task<PaginatedResult<AnswerList>> GetAnswerListPage(AnswerStatusEnum? status, SortDirection sortDirection, PaginatedRequest paginatedRequest)
        {
            string mainSql = @"
                SELECT a.Id, a.Title, ast.Name AS Status
                FROM Answers a 
                LEFT JOIN AnswerStatuses ast ON a.StatusId = ast.Id
            ";

            return GetAnswerPage<AnswerList>(mainSql, status, sortDirection, paginatedRequest);
        }

        public async Task<IEnumerable<AnswerStatusFilter>> GetAnswerStatusFilters()
        {
            string sql = @"
                SELECT Id, Name
                FROM AnswerStatuses
            ";

            using var connection = GetConnection();

            var statuses = await connection.QueryAsync<AnswerStatusFilter>(sql);
            
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
    }
}
