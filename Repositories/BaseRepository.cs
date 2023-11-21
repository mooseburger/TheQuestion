using Microsoft.Data.SqlClient;

namespace TheQuestion.Repositories
{
    public class BaseRepository
    {
        private readonly IConfiguration _configuration;
        public BaseRepository(IConfiguration config) 
        { 
            _configuration = config;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
}
