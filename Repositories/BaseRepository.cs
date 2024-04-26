using Npgsql;
using System.Data;

namespace TheQuestion.Repositories
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;
        public BaseRepository(IConfiguration config) 
        { 
            _configuration = config;
        }

        protected IDbConnection GetConnection()
        {
            return new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
