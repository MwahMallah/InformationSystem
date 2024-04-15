using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Tests;
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
    public async Task AddActivity()
    {
        
        var activity = new ActivityDetailModel
        {
            Id = Guid.NewGuid(), // Id will be set upon saving
            Description = "Test activity",
            CourseAbbreviation = "IDS",
        };

        // Act: Attempt to save the new activity
        activity = await _activityFacadeSUT.SaveAsync(activity);

        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var activityFromDb = await dbxAssert.Activities.SingleAsync(i => i.Id == activity.Id);
        DeepAssert.Equal(activity, ActivityModelMapper.MapToDetailModel(activityFromDb));
    } 
    
}