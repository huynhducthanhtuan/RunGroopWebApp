using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            List<AppUser> users = _context.Users.ToList();
            List<UserViewModel> userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                UserViewModel userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                };
                userViewModels.Add(userViewModel);
            }
            return userViewModels;
        }

        public async Task<UserDetailViewModel> GetUserById(string id)
        {
            AppUser user = _context.Users.Find(id);
            UserDetailViewModel userDetailViewModel = new UserDetailViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage,
            };
            return userDetailViewModel;
        }

        public bool Add(AppUser user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Update(AppUser user)
        {
            _context.Update(user);
            return Save();
        }

        public bool Delete(AppUser user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
