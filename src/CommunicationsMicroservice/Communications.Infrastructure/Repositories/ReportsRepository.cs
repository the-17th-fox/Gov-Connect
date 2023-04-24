using Communications.Core.Interfaces;
using Communications.Core.Misc;
using Communications.Core.Models;
using Communications.Infrastructure.DbContext;
using Communications.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Communications.Infrastructure.Repositories;

public class ReportsRepository : GenericRepository<Report>, IReportsRepository
{
    public ReportsRepository(CommunicationsDbContext dbContext) : base(dbContext)
    {
    }

    public new async Task<Report?> GetByIdAsync(Guid id)
    {
        return await EntityTable
            .Include(r => r.Classifications)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public override async Task<bool> CheckIfExistsAsync(Guid id)
    {
        return await EntityTable.AnyAsync(c => c.Id == id);
    }

    public override async Task<List<Report>> GetAllAsync(short pageNumber, byte pageSize)
    {
        var query = EntityTable
            .AsNoTracking()
            .Include(n => n.Classifications);

        return await GetAllByQueryAsync(query, pageNumber, pageSize);
    }

    public async Task<List<Report>> GetAllByCivilianAsync(Guid civilianId, short pageNumber, byte pageSize)
    {
        var query = EntityTable
            .AsNoTracking()
            .Where(r => r.CivilianId == civilianId);

        return await GetAllByQueryAsync(query, pageNumber, pageSize);
    }

    public async Task<List<Report>> GetAllPendingAsync(short pageNumber, byte pageSize)
    {
        var query = EntityTable
            .AsNoTracking()
            .Where(r => r.ReportStatus == ReportStatuses.Pending);

        return await GetAllByQueryAsync(query, pageNumber, pageSize);
    }

    private static async Task<List<Report>> GetAllByQueryAsync(IQueryable<Report> query, short pageNumber, byte pageSize)
    {
        return await PagedList<Report>
            .ToPagedListAsync(query, pageNumber, pageSize, r => r.Id);
    }
}