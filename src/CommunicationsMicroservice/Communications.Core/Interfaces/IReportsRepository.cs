using Communications.Core.Models;

namespace Communications.Core.Interfaces;

public interface IReportsRepository : IGenericRepository<Report>
{
    public Task<List<Report>> GetAllByCivilianAsync(Guid civilianId, short pageNumber, byte pageSize);
    public Task<List<Report>> GetAllPendingAsync(short pageNumber, byte pageSize);
    public new Task<Report?> GetByIdAsync(Guid id, bool includeReply);
}