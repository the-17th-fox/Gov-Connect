using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Civilians.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.Repositories
{
    public class PassportsRepository : IPassportsRepository
    {
        private readonly CiviliansDbContext _dbContext;

        public PassportsRepository(CiviliansDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public void Create(Passport passport)
            => _dbContext.Passports.Add(passport);

        public async Task<Passport?> GetByPersonalInfoAsync(string firstName, string lastName, string patronymic)
        {
            return await _dbContext.Passports
                .Where(p => p.FirstName == firstName)
                .Where(p => p.LastName == lastName)
                .Where(p => p.Patronymic == patronymic).FirstOrDefaultAsync();
        }

        public async Task<Passport?> GetByIdAsync(Guid id)
            => await _dbContext.Passports.FindAsync(id);

        public void Update(Passport passport)
            => _dbContext.Passports.Update(passport);
    }
}
