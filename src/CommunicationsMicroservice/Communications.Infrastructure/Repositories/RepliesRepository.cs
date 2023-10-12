using Communications.Core.Interfaces;
using Communications.Core.Models;
using Communications.Infrastructure.DbContext;
using Communications.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Communications.Infrastructure.Repositories;

public class RepliesRepository : GenericRepository<Reply>, IRepliesRepository
{
    public RepliesRepository(CommunicationsDbContext dbContext) : base(dbContext)
    {
    }

    public override async Task<List<Reply>> GetAllAsync(short pageNumber, byte pageSize)
    {
        var query = EntityTable.AsNoTracking();

        return await PagedList<Reply>
            .ToPagedListAsync(query, pageNumber, pageSize, sortingExpression: n => n.CreatedAt);
    }

    public override async Task<bool> CheckIfExistsAsync(Guid id)
    {
        return await EntityTable.AnyAsync(c => c.Id == id);
    }

    public async Task<Reply?> GetForReportAsync(Guid reportId)
    {
        return await EntityTable.FirstOrDefaultAsync(r => r.ReportId == reportId);
    }
}