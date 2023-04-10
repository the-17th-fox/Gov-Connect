namespace Civilians.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public IPassportsRepository PassportsRepository { get; }
        public ITokensRepository TokensRepository { get; }
        public IUsersRepository UsersRepository { get; }
        public Task SaveChangesAsync();
    }
}
