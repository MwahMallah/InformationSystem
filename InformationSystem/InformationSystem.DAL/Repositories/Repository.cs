using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL.Repositories;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private IEntityMapper<TEntity> _entityMapper;
    private DbContext _dbContext;
    
    public Repository(DbContext dbContext, IEntityMapper<TEntity> entityMapper)
    {
        _entityMapper = entityMapper;
        _dbSet = dbContext.Set<TEntity>();
        _dbContext = dbContext;
    }
    
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        return (await _dbSet.AddAsync(entity)).Entity;
    }

    public IQueryable<TEntity> Get()
    {
        return _dbSet.AsQueryable();
    }
    
    public async ValueTask<bool> ExistsAsync(TEntity entity)
    {
        return entity.Id != Guid.Empty && 
               (await _dbSet.AnyAsync(e => e.Id == entity.Id));
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var existingEntity = await _dbSet.SingleAsync(e => e.Id == entity.Id);
        _entityMapper.MapToExistingEntity(existingEntity, entity);
        return existingEntity;
    }

    public void Delete(Guid entityId)
    {
        _dbSet.Remove(_dbSet.Single(e => e.Id == entityId));
    }
}