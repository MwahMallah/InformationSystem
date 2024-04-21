using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Enums;
using InformationSystem.Common.Tests;
using InformationSystem.Common.Tests.Seeds;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace InformationSystem.BL.Tests;

public class ActivityFacadeTests: FacadeTestBase
{
    private readonly ActivityFacade _activityFacadeSUT;
    
    public ActivityFacadeTests(ITestOutputHelper output) : base(output)
    {
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
    }
    
    [Fact]
    public async Task AddActivityWithoutCourseThrows()
    {
        var activity = new ActivityDetailModel
        {
            Description = "Test activity",
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };

        await Assert.ThrowsAsync<InvalidOperationException>(async()=> 
            await _activityFacadeSUT.SaveAsync(activity)
        );
    } 
    
    [Fact]
    public async Task AddActivityWithCourse()
    {
        var activity = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };

        // Act: Attempt to save the new activity
        activity = await _activityFacadeSUT.SaveAsync(activity);
        Assert.Equal("ICS", activity.CourseAbbreviation);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var activityFromDb = await dbxAssert.Activities
                                .Include(a=>a.Course)
                                .SingleAsync(i => i.Id == activity.Id);
        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
    } 
    
    [Fact]
    public async Task GetAll_ActivityFindWithGivenId()
    {
        var activity = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        activity = await _activityFacadeSUT.SaveAsync(activity);
        
        var activities = await _activityFacadeSUT.GetAsync();
        var dbActivity = activities.SingleOrDefault(s => s.Id == activity.Id);
        Assert.Equal(activity.Id, dbActivity.Id);
    }
    
    
    [Fact]
    public async Task GetById_Activity()
    {
        var activity = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        activity = await _activityFacadeSUT.SaveAsync(activity);
        var dbActivity = await _activityFacadeSUT.GetAsync(activity.Id);
        DeepAssert.Equal(activity, dbActivity);
    }
    
    [Fact]
    public async Task GetById_NonExistentActivity()
    {
        var nonExistentStudent = await _activityFacadeSUT.GetAsync(Guid.Empty);
        Assert.Null(nonExistentStudent);
    }
    
    
    [Fact]
    public async Task Update_ActivityUpdated()
    {
        var activity = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        activity = await _activityFacadeSUT.SaveAsync(activity);
        activity.Description = "New description";
        
        activity = await _activityFacadeSUT.SaveAsync(activity);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var dbActivity = await dbxAssert.Activities
                    .Include(a=>a.Course)
                    .SingleAsync(a => a.Id == activity.Id);
        
        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(dbActivity));
    }
    
    
    [Fact]
    public async Task DeleteById_ActivityDeleted()
    {
        var activity = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        activity = await _activityFacadeSUT.SaveAsync(activity);
        
        await _activityFacadeSUT.DeleteAsync(activity.Id);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var found = await dbxAssert.Activities.AnyAsync(a => a.Id == activity.Id);
        Assert.False(found);
    }
    
    [Fact]
    public async Task DeleteById_NonExistentActivityThrows()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async()=>
            await _activityFacadeSUT.DeleteAsync(Guid.Empty)
        );
    }
}