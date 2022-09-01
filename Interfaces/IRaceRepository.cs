using MyProjectRunGroup.Models;

namespace MyProjectRunGroup.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAll();
        Task<IEnumerable<Race>> GetAllRaceByCity(string city);
        Task<Race> GetByIdAsync(int id);
        Task<Race> GetByIdAsyncAsNoTracking(int id);
        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);
        bool Save();
    }
}
