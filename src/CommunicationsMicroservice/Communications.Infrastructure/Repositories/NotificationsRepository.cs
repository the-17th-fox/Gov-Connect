using Communications.Core.Interfaces;
using Communications.Core.Models;
using Communications.Infrastructure.DbContext;
using Communications.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Communications.Infrastructure.Repositories;

public class NotificationsRepository : GenericRepository<Notification>, INotificationsRepository
{
    public NotificationsRepository(CommunicationsDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<List<Notification>> GetAllAsync(short pageNumber, byte pageSize)
    {
        var query = EntityTable
            .AsNoTracking()
            .Include(n => n.Classifications);

        return await PagedList<Notification>
            .ToPagedListAsync(query, pageNumber, pageSize, sortingExpression: n => n.Id);
    }

    public override async Task<bool> CheckIfExistsAsync(Guid id)
    {
        return await EntityTable.AnyAsync(c => c.Id == id);
    }
}