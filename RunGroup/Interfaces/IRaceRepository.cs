using RunGroup.Models;
using RunGroup.ViewModels;

namespace RunGroup.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllRaces();
        Task<RaceViewModel> GetRaceById(int id);
        Task<IEnumerable<Race>> GetRacesByCity(string city);
        Task<bool> Add(Race race);
        Task<bool> Update(Race race);
        Task<bool> Delete(int id);
    }
}
