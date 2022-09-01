using Microsoft.EntityFrameworkCore;

using MyProjectRunGroup.Data;
using MyProjectRunGroup.Interfaces;
using MyProjectRunGroup.Models;

namespace MyProjectRunGroup.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ClubRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _dbContext.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _dbContext.Clubs.Include(address => address.Address).FirstOrDefaultAsync(club => club.Id == id);
        }

        public async Task<Club> GetByIdAsyncAsNoTracking(int id)
        {
            return await _dbContext.Clubs.Include(address => address.Address).AsNoTracking().FirstOrDefaultAsync(club => club.Id == id);
        }

        public async Task<IEnumerable<Club>> GetAllClubByCity(string city)
        {
            return await _dbContext.Clubs.Where(club => club.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            return _dbContext.SaveChanges() > 0 ? true : false;
        }

        public bool Update(Club club)
        {
            _dbContext.Update(club);
            return Save();
        }

        public bool Add(Club club)
        {
            _dbContext.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _dbContext.Remove(club);
            return Save();
        }
    }
}
