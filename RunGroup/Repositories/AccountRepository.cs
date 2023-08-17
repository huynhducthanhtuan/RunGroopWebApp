using Dapper;
using Microsoft.Data.SqlClient;
using RunGroup.Interfaces;
using RunGroup.ViewModels;
using System.Data;

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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int count = connection.ExecuteScalar<int>(
                        "CheckExistedEmail",
                        new { Email = email },
                        commandType: CommandType.StoredProcedure
                    );

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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    int count = connection.ExecuteScalar<int>(
                        "CheckExistedAccount",
                        new
                        {
                            Email = loginViewModel.EmailAddress,
                            Password = loginViewModel.Password
                        },
                        commandType: CommandType.StoredProcedure
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
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    
                    string autoId = Guid.NewGuid().ToString("D");

                    int affectedRows = connection.Execute(
                        "Register",
                        new
                        {
                            Id = autoId,
                            Email = registerViewModel.EmailAddress,
                            Password = registerViewModel.Password,
                            UserName = registerViewModel.EmailAddress.Split('@')[0]
                        },
                        commandType: CommandType.StoredProcedure
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
