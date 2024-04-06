using InformationSystem.Common.Enums;
using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;
using InformationSystem.Common.Tests;
using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Tests;

public class DbContextActivityTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Activity_Persisted()
    {
        var activity = ActivitySeeds.ActivityEntity;

        InformationSystemDbContextSUT.Activities.Add(activity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbActivity = await dbx.Activities
            .SingleAsync(i => i.Id == activity.Id);
        DeepAssert.Equal(activity, dbActivity);
    }

    [Fact]
    public async Task GetById_Activity()
    {
        var activity = await InformationSystemDbContextSUT.Activities
            .SingleAsync(i => i.Id == ActivitySeeds.ActivityEntity.Id);

        //Assert
        DeepAssert.Equal(ActivitySeeds.ActivityEntity, activity);
    }

    [Fact]
    public async Task Update_Activity_Persisted()
    {
        var baseEntity = ActivitySeeds.ActivityEntity;
        var entity =
            baseEntity with
            {
                ActivityType = ActivityType.Exercise,
                RoomType = RoomType.MathClass,
            };

        InformationSystemDbContextSUT.Activities.Update(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Activities.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Activity_Deleted()
    {
        var baseEntity = ActivitySeeds.ActivityEntity;

        InformationSystemDbContextSUT.Activities.Remove(baseEntity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await InformationSystemDbContextSUT.Activities.AnyAsync(i => i.Id == baseEntity.Id));
    }

    [Fact]
    public async Task GetAll_Activities_ContainsSeededActivity()
    {
        var activities = await InformationSystemDbContextSUT.Activities.ToListAsync();

        //Assert
        DeepAssert.Contains(ActivitySeeds.ActivityEntity, activities);
    }

    [Fact]
    public async Task GetById_IncludingCourse_Activity()
    {
        var entity = await InformationSystemDbContextSUT.Activities
            .Include(i => i.Course)
            .SingleAsync(i => i.Id == ActivitySeeds.ActivityEntity.Id);

        //Assert
        DeepAssert.Equal(ActivitySeeds.ActivityEntity, entity);
    }
}