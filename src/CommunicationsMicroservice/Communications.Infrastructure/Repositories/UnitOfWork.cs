using Communications.Core.Interfaces;
using Communications.Infrastructure.DbContext;

namespace Communications.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CommunicationsDbContext _dbContext;

    private IClassificationsRepository _classificationsRepository = null!;
    private INotificationsRepository _notificationsRepository = null!;
    private IRepliesRepository _repliesRepository = null!;
    private IReportsRepository _reportsRepository = null!;

    public UnitOfWork(CommunicationsDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public IClassificationsRepository ClassificationsRepository => _classificationsRepository ??= new ClassificationsRepository(_dbContext);

    public INotificationsRepository NotificationsRepository => _notificationsRepository ??= new NotificationsRepository(_dbContext);

    public IRepliesRepository RepliesRepository => _repliesRepository ??= new RepliesRepository(_dbContext);

    public IReportsRepository ReportsRepository => _reportsRepository ??= new ReportsRepository(_dbContext);

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}