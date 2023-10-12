namespace Authorities.Core.Interfaces
{
    public interface IUnitOfWork
    {
        public ITokensRepository TokensRepository { get; }
        public IUsersRepository UsersRepository { get; }
        public Task SaveChangesAsync();
    }
}
