using Communications.Core.Interfaces;
using Communications.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Communications.Infrastructure.Repositories;

public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    protected readonly DbSet<TEntity> EntityTable;

    protected GenericRepository(CommunicationsDbContext dbContext)
    {
        var context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        EntityTable = context.Set<TEntity>();
    }

    public void Create(TEntity entity)
    {
        EntityTable.Add(entity);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await EntityTable.FindAsync(id);
    }

    public void Update(TEntity entity)
    {
        EntityTable.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        EntityTable.Remove(entity);
    }

    public abstract Task<List<TEntity>> GetAllAsync(short pageNumber, byte pageSize);

    public abstract Task<bool> CheckIfExistsAsync(Guid id);
}