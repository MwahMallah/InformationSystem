using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace InformationSystem.DAL.UnitOfWork;

public class UnitOfWorkFactory: IUnitOfWorkFactory
{
    private readonly IDbContextFactory<InformationSystemDbContext> _dbContextFactory;
    
    public UnitOfWorkFactory(IDbContextFactory<InformationSystemDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public IUnitOfWork Create()
    {
        var dbContext = _dbContextFactory.CreateDbContext();
        return new UnitOfWork(dbContext);
    }
}