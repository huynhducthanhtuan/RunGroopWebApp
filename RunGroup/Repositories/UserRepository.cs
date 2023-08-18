using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RunGroup.Interfaces;
using RunGroup.ViewModels;
using System.Data;

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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<UserViewModel> users = connection.Query<UserViewModel>(
                        "sp_GetAllUsers",
                        commandType: CommandType.StoredProcedure
                    );
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    UserDetailViewModel user = connection.QueryFirstOrDefault<UserDetailViewModel>(
                        "sp_GetUserById",
                        new { id = id },
                        commandType: CommandType.StoredProcedure
                    );
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int effectedRaceRows = connection.Execute(
                        "sp_UpdateUser",
                        new
                        {
                            Id = user.Id,
                            Pace = user.Pace,
                            Mileage = user.Mileage,
                            Street = user.Street,
                            City = user.City,
                            State = user.State
                        },
                        commandType: CommandType.StoredProcedure
                    );
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int effectedRaceRows = connection.Execute(
                        "sp_UpdateUserProfileImageUrl",
                        new
                        {
                            Id = userId,
                            ProfileImageUrl = profileImageUrl
                        },
                        commandType: CommandType.StoredProcedure
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
