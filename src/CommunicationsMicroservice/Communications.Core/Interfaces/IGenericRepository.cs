namespace Communications.Core.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : class
{
    public void Create(TEntity entity);
    public Task<TEntity?> GetByIdAsync(Guid id);
    public Task<List<TEntity>> GetAllAsync(short pageNumber, byte pageSize);
    public void Update(TEntity entity);
    public void Delete(TEntity entity);
}