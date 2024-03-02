using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL.Factories;

public class DbContextSqliteFactory: IDbContextFactory<InformationSystemDbContext>
{
    private readonly DbContextOptionsBuilder <InformationSystemDbContext> _optionsBuilder = new();

    public DbContextSqliteFactory(string databaseName)
    {
        _optionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");

    }
    public InformationSystemDbContext CreateDbContext() => new InformationSystemDbContext(_optionsBuilder.Options);
}