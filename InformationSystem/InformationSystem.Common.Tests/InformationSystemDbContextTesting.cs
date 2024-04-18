// using CookBook.Common.Tests.Seeds;

using InformationSystem.Common.Tests.Seeds;
using InformationSystem.DAL;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests;

public class InformationSystemDbContextTesting(DbContextOptions contextOptions, bool seedTestingData = false)
    : InformationSystemDbContext(contextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        if (seedTestingData)
        {
            CourseSeeds.Seed(modelBuilder);
            StudentSeeds.Seed(modelBuilder);
            ActivitySeeds.Seed(modelBuilder);
        }
    }
}