using RunGroup.ViewModels;

namespace RunGroup.Interfaces
{
    public interface IAccountRepository
    {
        Task<bool> Register(RegisterViewModel registerViewModel);
        Task<bool> CheckExistedEmail(string email);
        Task<bool> CheckExistedAccount(LoginViewModel loginViewModel);
    }
}
