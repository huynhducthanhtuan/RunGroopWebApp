using RunGroup.Models;

namespace RunGroup.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAllClubs();
        Task<Club> GetClubById(int id);
        Task<Club> GetClubByIdNoTracking(int id);
        Task<IEnumerable<Club>> GetClubsByCity(string city);
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();
    }
}
