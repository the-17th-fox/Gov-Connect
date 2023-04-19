using Communications.Core.Models;

namespace Communications.Core.Interfaces;

public interface IRepliesRepository : IGenericRepository<Reply>
{
    public Task<Reply?> GetForReportAsync(Guid reportId);
}