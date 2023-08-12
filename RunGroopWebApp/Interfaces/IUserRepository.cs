using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Interfaces
{
    public interface IUserRepository
    {
        Task<List<UserViewModel>> GetAllUsers();
        Task<UserDetailViewModel> GetUserById(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
