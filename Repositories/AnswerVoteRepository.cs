using Dapper;
using System.Data;

namespace TheQuestion.Repositories
{
    public interface IAnswerVoteRepository
    {
        Task<Tuple<double, int>> AddVote(int answerId, string ipAddress);

        Task<Tuple<double, int>> RemoveVote(int answerId, string ipAddress);

        Task<IEnumerable<int>> GetVotesByIpAddress(string ipAddress);
    }

    public class AnswerVoteRepository : BaseRepository, IAnswerVoteRepository
    {
        public AnswerVoteRepository(IConfiguration configuration) : base(configuration) { }

        public async Task<Tuple<double, int>> AddVote(int answerId, string ipAddress)
        {
            using var connection = GetConnection();

            string sql = @"INSERT INTO ""AnswerVotes""
            (""AnswerId"", ""IpAddress"", ""VoteDate"")
            VALUES
            (@answerId, @ipAddress, @voteDate)
            ON CONFLICT DO UPDATE
            SET ""VoteDate"" = @voteDate";

            await connection.ExecuteAsync(sql, new { answerId, ipAddress, voteDate = DateTimeOffset.UtcNow });

            return await UpdateRank(connection, answerId);
        }

        public async Task<Tuple<double, int>> RemoveVote(int answerId, string ipAddress)
        {
            using var connection = GetConnection();

            string sql = @"DELETE FROM ""AnswerVotes"" WHERE ""AnswerId"" = @answerId AND ""IpAddress"" = @ipAddress";
            await connection.ExecuteAsync(sql, new { answerId, ipAddress });

            return await UpdateRank(connection, answerId);
        }

        private async Task<Tuple<double, int>> UpdateRank(IDbConnection connection, int answerId)
        {
            string sql = @"SELECT COUNT(*) FROM ""AnswerVotes"" WHERE ""AnswerId"" = @answerId";
            int totalVotes = await connection.ExecuteScalarAsync<int>(sql, new { answerId });

            // Popularity wave! More votes can put you higher than anything else, but it cycles, and answers with less votes can appear ahead
            // Hammered out the details using Wolfram Alpha
            double rank = (0.5 * totalVotes * Math.Cos(0.05 * totalVotes)) + (0.65 * totalVotes);

            sql = @"UPDATE ""Answers"" 
                SET ""Rank"" = @rank, ""TotalVotes"" = @totalVotes
                WHERE ""AnswerId"" = @answerId 
            ";

            await connection.ExecuteAsync(sql, new { answerId, rank, totalVotes });
        
            return Tuple.Create(rank, totalVotes);
        }

        public Task<IEnumerable<int>> GetVotesByIpAddress(string ipAddress)
        {
            using var connection = GetConnection();

            string sql = @"SELECT ""AnswerId"" FROM ""AnswerVotes"" WHERE ""IpAddress"" = @ipAddress";

            return connection.QueryAsync<int>(sql, new { ipAddress });
        }
    }
}
