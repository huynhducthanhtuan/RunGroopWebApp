using RunGroup.Models;

namespace RunGroup.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<Club>> GetAllUserClubs();
        Task<IEnumerable<Race>> GetAllUserRaces();
    }
}
