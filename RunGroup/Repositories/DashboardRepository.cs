using Dapper;
using Microsoft.Data.SqlClient;
using RunGroup.Helpers;
using RunGroup.Interfaces;
using RunGroup.Models;
using System.Data;

namespace RunGroup.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string connectionString;

        public DashboardRepository(
            IConfiguration configuration, 
            IHttpContextAccessor httpContextAccessor
        )
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Club>> GetAllUserClubs()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string userId = _httpContextAccessor.HttpContext.User.GetUserId();
                    IEnumerable<Club> clubs = connection.Query<Club>(
                        "sp_GetAllUserClubs",
                        new { id = userId },
                        commandType: CommandType.StoredProcedure
                    );
                    return clubs;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<IEnumerable<Race>> GetAllUserRaces()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string userId = _httpContextAccessor.HttpContext.User.GetUserId();
                    IEnumerable<Race> races = connection.Query<Race>(
                        "sp_GetAllUserRaces", 
                        new { id = userId },
                        commandType: CommandType.StoredProcedure
                    );
                    return races;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
