using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace InformationSystem.DAL.UnitOfWork;

public class UnitOfWorkFactory(IDbContextFactory<InformationSystemDbContext> dbContextFactory)
    : IUnitOfWorkFactory
{
    public IUnitOfWork Create()
    {
        var dbContext = dbContextFactory.CreateDbContext();
        return new UnitOfWork(dbContext);
    }
}