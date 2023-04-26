using Communications.Core.Interfaces;
using Communications.Core.Models;
using Communications.Infrastructure.DbContext;
using Communications.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Communications.Infrastructure.Repositories;

public class ClassificationsRepository : GenericRepository<Classification>, IClassificationsRepository
{
    public ClassificationsRepository(CommunicationsDbContext dbContext) : base(dbContext) {}


    public override async Task<List<Classification>> GetAllAsync(short pageNumber, byte pageSize)
    {
        var query = EntityTable.AsNoTracking();

        return await PagedList<Classification>
            .ToPagedListAsync(query, pageNumber, pageSize, sortingExpression: c => c.Id);
    }

    public override async Task<bool> CheckIfExistsAsync(Guid id)
    {
        return await EntityTable.AnyAsync(c => c.Id == id);
    }

    public async Task<Classification?> GetByNameAsync(string name)
    {
        return await EntityTable.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == name);
    }
}