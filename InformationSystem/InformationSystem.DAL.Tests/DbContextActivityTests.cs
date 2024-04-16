using InformationSystem.Common.Tests;
using InformationSystem.Common.Tests.Seeds;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;

namespace InformationSystem.DAL.Tests;

public class DbContextActivityTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_ActivityWithCourses_Persisted()
    {
        var entity = new ActivityEntity()
        {
            Id = Guid.NewGuid(),
            Description = "Hello world"
        };

        InformationSystemDbContextSUT.Activities.Add(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbEntity = await dbx.Activities
            .SingleAsync(i => i.Id == entity.Id);
        
        DeepAssert.Equal(entity, dbEntity);
    }
}