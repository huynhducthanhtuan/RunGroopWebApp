using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repositories
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _context;

        public RaceRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task<IEnumerable<Race>> GetAllRaces()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetRaceById(int id)
        {
            return await _context.Races
                .Include(r => r.Address)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Race> GetRaceByIdNoTracking(int id)
        {
            return await _context.Races
                .Include(r => r.Address)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Race>> GetRacesByCity(string city)
        {
            return await _context.Races
                .Include(r => r.Address)
                .Where(r => r.Address.City == city)
                .ToListAsync();
        }

        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
