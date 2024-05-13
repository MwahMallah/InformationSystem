using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL.Factories;

public class DbContextSqliteFactory: IDbContextFactory<InformationSystemDbContext>
{
    private readonly DbContextOptionsBuilder<InformationSystemDbContext> _optionsBuilder = new();
    private readonly bool _seedTestingData;

    public DbContextSqliteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;
        
        _optionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");
    }
    public InformationSystemDbContext CreateDbContext() => new(_optionsBuilder.Options, _seedTestingData);
}