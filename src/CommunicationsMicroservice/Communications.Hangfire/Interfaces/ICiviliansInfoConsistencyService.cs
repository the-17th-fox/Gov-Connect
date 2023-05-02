namespace Communications.Hangfire.Interfaces;

public interface ICiviliansInfoConsistencyService
{
    public Task UpdateCiviliansInfoAsync(CancellationToken cancellationToken);
}