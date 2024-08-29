using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;
using System.Data;

namespace RunGroup.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public RaceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Race>> GetAllRaces()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Race> races = connection.Query<Race>(
                        "sp_GetAllRaces",
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

        public async Task<RaceViewModel> GetRaceById(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    RaceViewModel race = connection.QueryFirstOrDefault<RaceViewModel>(
                        "sp_GetRaceById",
                        new { id = id },
                        commandType: CommandType.StoredProcedure
                    );
                    return race;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public async Task<IEnumerable<Race>> GetRacesByCity(string city)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Race> races = connection.Query<Race>(
                        "sp_GetRacesByCity",
                        new { city = city },
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

        public async Task<bool> Add(Race race)
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
                            Street = race.Address.Street,
                            City = race.Address.City,
                            State = race.Address.State
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    // If insert fail
                    if (addressId == 0) return false;

                    race.AddressId = addressId;

                    // Insert Race
                    int effectedRows = await connection.ExecuteAsync(
                        "sp_AddRace",
                        new
                        {
                            Title = race.Title,
                            Description = race.Description,
                            Image = race.Image,
                            RaceCategory = race.RaceCategory,
                            AddressId = race.AddressId,
                            AppUserId = race.AppUserId,
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

        public async Task<bool> Update(Race race)
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
                            Id = race.Address.Id,
                            Street = race.Address.Street,
                            City = race.Address.City,
                            State = race.Address.State
                        },
                        commandType: CommandType.StoredProcedure
                    );

                    // If update fail
                    if (effectedAddressRows == 0) return false;

                    // Update Race
                    int effectedRaceRows = connection.Execute(
                        "sp_UpdateRace",
                        new
                        {
                            Id = race.Id,
                            Title = race.Title,
                            Description = race.Description,
                            Image = race.Image,
                            RaceCategory = race.RaceCategory
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

        public async Task<bool> Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int affectedRows = connection.Execute(
                        "sp_DeleteRace",
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
