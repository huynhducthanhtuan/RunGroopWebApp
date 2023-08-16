using Dapper;
using Microsoft.Data.SqlClient;
using RunGroup.Helpers;
using RunGroup.Interfaces;
using RunGroup.Models;

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
            string sql =
                @"SELECT c.Id, Title, Description, Image, ClubCategory
                FROM Clubs AS c
                LEFT JOIN Addresses AS a ON c.AddressId = a.Id
                WHERE c.AppUserId = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string userId = _httpContextAccessor.HttpContext.User.GetUserId();
                    IEnumerable<Club> clubs = connection.Query<Club>(sql, new { id = userId });
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
            string sql =
                @"SELECT r.Id, Title, Description, Image, RaceCategory
                FROM Races AS r
                LEFT JOIN Addresses AS a ON r.AddressId = a.Id
                WHERE r.AppUserId = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string userId = _httpContextAccessor.HttpContext.User.GetUserId();
                    IEnumerable<Race> races = connection.Query<Race>(sql, new { id = userId });
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
