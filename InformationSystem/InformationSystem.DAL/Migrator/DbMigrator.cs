using InformationSystem.DAL.Options;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL.Migrator;

public class DbMigrator(IDbContextFactory<InformationSystemDbContext> dbContextFactory, DALOptions options)
    : IDbMigrator
{
    public void Migrate()
    {
        MigrateAsync(CancellationToken.None).GetAwaiter().GetResult();
    }

    public async Task MigrateAsync(CancellationToken cancellationToken)
    {
        await using InformationSystemDbContext dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var str = options.DatabaseFilePath;
        
        if (options.RecreateDatabaseEachTime)
        {
            await dbContext.Database.EnsureDeletedAsync(cancellationToken);
        }

        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
    }
}