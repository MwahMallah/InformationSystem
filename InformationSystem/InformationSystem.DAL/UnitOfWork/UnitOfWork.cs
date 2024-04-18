using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _dbContext;
    
    public UnitOfWork(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public IRepository<TEntity> GetRepository<TEntity, TEntityMapper>() 
        where TEntity : class, IEntity where TEntityMapper : IEntityMapper<TEntity>, new()
    {
        return new Repository<TEntity>(_dbContext, new TEntityMapper());
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _dbContext.DisposeAsync();
    }
}