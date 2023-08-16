using RunGroup.Models;
using RunGroup.ViewModels;

namespace RunGroup.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAllClubs();
        Task<ClubViewModel> GetClubById(int id);
        Task<IEnumerable<Club>> GetClubsByCity(string city);
        Task<bool> Add(Club club);
        Task<bool> Update(Club club);
        Task<bool> Delete(int id);
    }
}
