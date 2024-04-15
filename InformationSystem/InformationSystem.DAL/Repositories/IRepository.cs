using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Repositories;

public interface IRepository<TEntity> where TEntity: class, IEntity
{
    Task<TEntity> InsertAsync(TEntity entity);
    IQueryable<TEntity> Get();
    ValueTask<bool> ExistsAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity, Func<IQueryable<TEntity>, IQueryable<TEntity>>? include = null);
    void Delete(Guid entityId);
}