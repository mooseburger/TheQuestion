using Dapper;
using TheQuestion.Model.Generic;
using TheQuestion.Model.User;

namespace TheQuestion.Repositories
{
    public interface IUserRepository
    {
        Task<PaginatedResult<User>> GetUserPage(PaginatedRequest request);
    }

    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<PaginatedResult<User>> GetUserPage(PaginatedRequest request)
        {
            using var connection = GetConnection();

            string pageSql = @"
                SELECT UserName AS Username, Email AS Email, r.Name AS RoleName
                FROM AspNetUsers u
                LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id
                INNER JOIN AspNetRoles r ON r.Id = ur.RoleId
                ORDER BY NormalizedUserName
                OFFSET @offset ROWS 
                FETCH NEXT @pageSize ROWS ONLY
            ";

            var page = await connection.QueryAsync<User>(pageSql, new { 
                offset = request.Offset,
                pageSize = request.PageSize
            });

            string totalResultsSql = "SELECT COUNT(*) FROM AspNetUsers";
            int totalResults = await connection.ExecuteScalarAsync<int>(totalResultsSql);

            return new PaginatedResult<User>
            {
                Page = page,
                TotalRecords = totalResults
            };
        }
    }
}
