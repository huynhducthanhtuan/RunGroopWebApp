using RunGroup.Models;

namespace RunGroup.Interfaces
{
    public interface IUserRepository
    {
        Task<List<AppUser>> GetAllUsers();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetUserByIdNoTracking(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
