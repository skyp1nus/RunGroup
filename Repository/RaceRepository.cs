using Microsoft.EntityFrameworkCore;

using MyProjectRunGroup.Data;
using MyProjectRunGroup.Interfaces;
using MyProjectRunGroup.Models;

namespace MyProjectRunGroup.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RaceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _dbContext.Races.ToListAsync();
        }

        public async Task<IEnumerable<Race>> GetAllRaceByCity(string city)
        {
            return await _dbContext.Races.Where(race => race.Address.City.Contains(city)).ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _dbContext.Races.Include(address => address.Address).FirstOrDefaultAsync(race => race.Id == id);
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool Update(Race race)
        {
            _dbContext.Update(race);
            return Save();
        }

        public bool Add(Race race)
        {
            _dbContext.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _dbContext.Remove(race);
            return Save();
        }
    }
}
