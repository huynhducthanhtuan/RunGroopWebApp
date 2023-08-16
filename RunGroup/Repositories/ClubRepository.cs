using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RunGroup.Data;
using RunGroup.Interfaces;
using RunGroup.Models;
using RunGroup.ViewModels;

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
            string sql = "SELECT * FROM Clubs";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Club> clubs = connection.Query<Club>(sql).ToList();
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
            string sql =
                @"SELECT c.Id, Title, Description, Image, ClubCategory, a.Id AS AddressId, 
                        Street AS AddressStreet, City AS AddressCity, State AS AddressState
                FROM Clubs AS c
                LEFT JOIN Addresses AS a ON c.AddressId = a.Id
                WHERE c.Id = @id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    ClubViewModel club = connection.QueryFirstOrDefault<ClubViewModel>(sql, new { id = id });
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
            string sql =
                @"SELECT c.Id, Title, Description, Image, ClubCategory
                FROM Clubs AS c
                LEFT JOIN Addresses AS a ON c.AddressId = a.Id
                WHERE a.City = @city";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    IEnumerable<Club> clubs = connection.Query<Club>(sql, new { city = city });
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
            string insertAddressSql = "INSERT INTO Addresses (Street, City, State) VALUES (@Street, @City, @State); " +
                                        "SELECT CAST(SCOPE_IDENTITY() as int)";
            string insertClubSql = "INSERT INTO Clubs (Title, Description, Image, ClubCategory, AddressId, AppUserId) " +
                                   "VALUES (@Title, @Description, @Image, @ClubCategory, @AddressId, @AppUserId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Insert Address & get inserted Address Id
                    int addressId = await connection.ExecuteScalarAsync<int>(insertAddressSql, club.Address);

                    // If insert fail
                    if (addressId == 0) return false;

                    club.AddressId = addressId;

                    // Insert Club
                    int effectedRows = await connection.ExecuteAsync(insertClubSql, club);
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
            string updateAddressSql = @"UPDATE Addresses 
                                        SET Street=@Street, City=@City, State=@State 
                                        WHERE Id=@Id";
            string updateClubSql = @"UPDATE Clubs 
                                     SET Title=@Title, Description=@Description, Image=@Image, ClubCategory=@ClubCategory 
                                     WHERE Id=@Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Update Address
                    int effectedAddressRows = connection.Execute(updateAddressSql, club.Address);

                    // If update fail
                    if (effectedAddressRows == 0) return false;

                    // Update Club
                    int effectedClubRows = connection.Execute(updateClubSql, club);
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
            string sql = @"DELETE FROM Clubs WHERE Id = @id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int affectedRows = connection.Execute(sql, new { id = id });
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
