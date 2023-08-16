using RunGroup.ViewModels;

namespace RunGroup.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserViewModel>> GetAllUsers();
        Task<UserDetailViewModel> GetUserById(string id);
        Task<bool> Update(UserDetailViewModel user);
        Task<bool> UpdateProfileImageUrl(string userId, string profileImageUrl);
    }
}
