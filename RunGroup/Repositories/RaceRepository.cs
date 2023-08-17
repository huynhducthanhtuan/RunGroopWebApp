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
            string sql = "SELECT * FROM Races";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Race> races = connection.Query<Race>(
                        "GetAllRaces",
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
            string sql =
                @"SELECT r.Id, Title, Description, Image, RaceCategory, a.Id AS AddressId, 
                        Street AS AddressStreet, City AS AddressCity, State AS AddressState
                FROM Races AS r
                LEFT JOIN Addresses AS a ON r.AddressId = a.Id
                WHERE r.Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    RaceViewModel race = connection.QueryFirstOrDefault<RaceViewModel>(
                        "GetRaceById",
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
            string sql =
                @"SELECT r.Id, Title, Description, Image, RaceCategory
                FROM Races AS r
                LEFT JOIN Addresses AS a ON r.AddressId = a.Id
                WHERE a.City = @city";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Race> races = connection.Query<Race>(
                        "GetRacesByCity",
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
            string insertAddressSql = @"INSERT INTO Addresses (Street, City, State)
                                        VALUES (@Street, @City, @State);
                                        SELECT CAST(SCOPE_IDENTITY() as int)";

            string insertRaceSql = @"INSERT INTO Races (Title, Description, Image, RaceCategory, AddressId, AppUserId)
                                     VALUES (@Title, @Description, @Image, @RaceCategory, @AddressId, @AppUserId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Insert Address & get inserted Address Id
                    int addressId = await connection.ExecuteScalarAsync<int>(
                        "AddAddress",
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
                        "AddRace",
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
            string updateAddressSql = @"UPDATE Addresses 
                                        SET Street=@Street, City=@City, State=@State 
                                        WHERE Id=@Id";

            string updateRaceSql = @"UPDATE Races 
                                     SET Title=@Title, Description=@Description, Image=@Image, RaceCategory=@RaceCategory 
                                     WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update Address
                    int effectedAddressRows = connection.Execute(
                        "UpdateAddress",
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
                        "UpdateRace",
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
            string sql = "DELETE FROM Races WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int affectedRows = connection.Execute(
                        "DeleteRace",
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
