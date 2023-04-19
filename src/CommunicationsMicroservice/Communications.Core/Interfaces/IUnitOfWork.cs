namespace Communications.Core.Interfaces;

public interface IUnitOfWork
{
    public IClassificationsRepository ClassificationsRepository { get; }
    public INotificationsRepository NotificationsRepository { get; }
    public IRepliesRepository RepliesRepository { get; }
    public IReportsRepository ReportsRepository { get; }

    public Task SaveChangesAsync();
}