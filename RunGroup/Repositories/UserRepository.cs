using Microsoft.EntityFrameworkCore;
using RunGroup.Data;
using RunGroup.Interfaces;
using RunGroup.Models;

namespace RunGroup.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AppUser>> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return _context.Users.Find(id);
        }

        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
            return _context.Users
                .Where(u => u.Id == id)
                .AsNoTracking()
                .FirstOrDefault();
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
