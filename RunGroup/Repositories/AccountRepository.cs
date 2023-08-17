using Dapper;
using Microsoft.Data.SqlClient;
using RunGroup.Interfaces;
using RunGroup.ViewModels;

namespace RunGroup.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IConfiguration _configuration;
        private string connectionString;

        public AccountRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> CheckExistedEmail(string email)
        {
            string sql = "SELECT COUNT(*) FROM AspNetUsers WHERE Email = @Email";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    int count = connection.ExecuteScalar<int>(sql, new { Email = email });
                    return count == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> CheckExistedAccount(LoginViewModel loginViewModel)
        {
            string sql = @"SELECT COUNT(*) FROM AspNetUsers
                           WHERE Email = @Email AND PasswordHash = @Password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int count = connection.ExecuteScalar<int>(
                        sql,
                        new
                        {
                            Email = loginViewModel.EmailAddress,
                            Password = loginViewModel.Password
                        }
                    );

                    return count == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public async Task<bool> Register(RegisterViewModel registerViewModel)
        {
            string sql = @"INSERT INTO AspNetUsers (Id, Email, PasswordHash, UserName, EmailConfirmed, PhoneNumberConfirmed, 
                                                    TwoFactorEnabled, LockoutEnabled, AccessFailedCount, Street) 
                           VALUES (@Id, @Email, @Password, @UserName, 0, 0, 0, 0, 0, '')";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    
                    string autoId = Guid.NewGuid().ToString("D");

                    int affectedRows = connection.Execute(
                        sql,
                        new
                        {
                            Id = autoId,
                            Email = registerViewModel.EmailAddress,
                            Password = registerViewModel.Password,
                            UserName = registerViewModel.EmailAddress.Split('@')[0]
                        }
                    );

                    return affectedRows == 1 ? true : false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
