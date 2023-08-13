using RunGroup.Data;
using RunGroup.Helpers;
using RunGroup.Interfaces;
using RunGroup.Models;

namespace RunGroup.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<Club>> GetAllUserClubs()
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var clubs = _context.Clubs.Where(c => c.AppUserId == userId).ToList();
            return clubs;
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var userId = _httpContextAccessor.HttpContext.User.GetUserId();
            var races = _context.Races.Where(r => r.AppUserId == userId).ToList();
            return races;
        }
    }
}
