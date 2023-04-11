using Civilians.Core.Interfaces;
using Civilians.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Civilians.Infrastructure.Repositories
{
    public class PassportsRepository : IPassportsRepository
    {
        private readonly CiviliansDbContext _context;

        public PassportsRepository(CiviliansDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Create(Passport passport)
            => _context.Passports.Add(passport);

        public async Task<Passport?> GetByPersonalInfoAsync(string firstName, string lastName, string patronymic)
        {
            return await _context.Passports
                .Where(p => p.FirstName == firstName)
                .Where(p => p.LastName == lastName)
                .Where(p => p.Patronymic == patronymic).FirstOrDefaultAsync();
        }

        public async Task<Passport?> GetByIdAsync(Guid id)
            => await _context.Passports.FindAsync(id);

        public void Update(Passport passport)
            => _context.Passports.Update(passport);
    }
}
