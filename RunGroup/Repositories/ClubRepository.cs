using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;
using System.Data;

namespace RunGroup.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public ClubRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Club>> GetAllClubs()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Club> clubs = connection.Query<Club>(
                        "sp_GetAllClubs", 
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

        public async Task<ClubViewModel> GetClubById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    ClubViewModel club = connection.QueryFirstOrDefault<ClubViewModel>(
                        "sp_GetClubById", 
                        new { id = id },
                        commandType: CommandType.StoredProcedure
                    );
                    return club;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<IEnumerable<Club>> GetClubsByCity(string city)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Club> clubs = connection.Query<Club>(
                        "sp_GetClubsByCity",
                        new { city = city },
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

        public async Task<bool> Add(Club club)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Insert Address & get inserted Address Id
                    int addressId = await connection.ExecuteScalarAsync<int>(
                        "sp_AddAddress", 
                        new
                        {
                            Street = club.Address.Street,
                            City = club.Address.City,
                            State = club.Address.State
                        }, 
                        commandType: CommandType.StoredProcedure
                    );

                    // If insert fail
                    if (addressId == 0) return false;

                    club.AddressId = addressId;

                    // Insert Club
                    int effectedRows = await connection.ExecuteAsync(
                        "sp_AddClub",
                        new
                        {
                            Title = club.Title,
                            Description = club.Description,
                            Image = club.Image,
                            ClubCategory = club.ClubCategory,
                            AddressId = club.AddressId,
                            AppUserId = club.AppUserId,
                        },
                        commandType: CommandType.StoredProcedure
                    );
                    return effectedRows == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> Update(Club club)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update Address
                    int effectedAddressRows = connection.Execute(
                        "sp_UpdateAddress",
                        new
                        {
                            Id = club.Address.Id,
                            Street = club.Address.Street,
                            City = club.Address.City,
                            State = club.Address.State
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    // If update fail
                    if (effectedAddressRows == 0) return false;

                    // Update Club
                    int effectedClubRows = connection.Execute(
                        "sp_UpdateClub",
                        new
                        {
                            Id = club.Id,
                            Title = club.Title,
                            Description = club.Description,
                            Image = club.Image,
                            ClubCategory = club.ClubCategory
                        },
                        commandType: CommandType.StoredProcedure
                    );
                    return effectedClubRows == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int affectedRows = connection.Execute(
                        "sp_DeleteClub", 
                        new { id = id },
                        commandType: CommandType.StoredProcedure
                    );
                    return affectedRows >= 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
