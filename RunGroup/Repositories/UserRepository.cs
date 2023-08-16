using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RunGroup.Interfaces;
using RunGroup.ViewModels;

namespace RunGroup.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsers()
        {
            string sql = "SELECT Id, UserName, ProfileImageUrl FROM AspNetUsers";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<UserViewModel> users = connection.Query<UserViewModel>(sql).ToList();
                    return users;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<UserDetailViewModel> GetUserById(string id)
        {
            string sql = @"SELECT Id, UserName, Pace, Mileage, ProfileImageUrl, Street, City, State 
                           FROM AspNetUsers WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    UserDetailViewModel user = connection.
                            QueryFirstOrDefault<UserDetailViewModel>(sql, new { id = id });
                    return user;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<bool> Update(UserDetailViewModel user)
        {
            string sql = @"UPDATE AspNetUsers
                           SET UserName=@UserName, Pace=@Pace, Mileage=@Mileage, ProfileImageUrl=@ProfileImageUrl, 
                               Street=@Street, City=@City, State=@State
                           WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int effectedRaceRows = connection.Execute(sql, user);
                    return effectedRaceRows == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> UpdateProfileImageUrl(string userId, string profileImageUrl)
        {
            string sql = "UPDATE AspNetUsers SET ProfileImageUrl=@ProfileImageUrl WHERE Id=@Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int effectedRaceRows = connection.Execute(
                        sql,
                        new
                        {
                            Id = userId,
                            ProfileImageUrl = profileImageUrl
                        }
                    );

                    return effectedRaceRows == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
