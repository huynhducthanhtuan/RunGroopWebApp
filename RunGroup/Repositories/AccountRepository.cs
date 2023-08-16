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
                           WHERE Email = @Email AND Password = @Password";
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
            string sql = @"INSERT INTO AspNetUsers (Email, Password) 
                           SET Email = @Email AND Password = @Password";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int affectedRows = connection.Execute(
                        sql,
                        new
                        {
                            Email = registerViewModel.EmailAddress,
                            Password = registerViewModel.Password,
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
