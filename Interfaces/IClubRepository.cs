using MyProjectRunGroup.Models;

namespace MyProjectRunGroup.Interfaces
{
    public interface IClubRepository
    {
        Task<IEnumerable<Club>> GetAll();
        Task<IEnumerable<Club>> GetAllClubByCity(string city);
        Task<Club> GetByIdAsync(int id);
        Task<Club> GetByIdAsyncAsNoTracking(int id);
        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);
        bool Save();
    }
}
