using Microsoft.EntityFrameworkCore;
using RunGroup.Data;
using RunGroup.Interfaces;
using RunGroup.Models;

namespace RunGroup.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Club>> GetAllClubs()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetClubById(int id)
        {
            return await _context.Clubs
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetClubByIdNoTracking(int id)
        {
            return await _context.Clubs
                .Include(c => c.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubsByCity(string city)
        {
            return await _context.Clubs
                .Include(c => c.Address)
                .Where(c => c.Address.City == city)
                .ToListAsync();
        }

        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
