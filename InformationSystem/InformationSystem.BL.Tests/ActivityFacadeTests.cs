using System.Globalization;
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
    public async Task GetAll_FilteredByAbbreviation()
    {
        var activity = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        await _activityFacadeSUT.SaveAsync(activity);
        var activities = await _activityFacadeSUT.GetAsync("IC");
        Assert.Single(activities);
        Assert.Equal("ICS", activities.ToList()[0].CourseAbbreviation);
    }
    
    [Fact]
    public async Task GetAll_FilteredByDescription()
    {
        var activities = await _activityFacadeSUT.GetAsync("c# ");
        Assert.Single(activities);
    }
    
    [Fact]
    public async Task GetAll_FilteredByStartDateInGivenCourse()
    {
        var activity1 = new ActivityDetailModel
        {
            Description = "Test activity1",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = DateTime.ParseExact("05-05-2023", "dd-MM-yyyy", CultureInfo.InvariantCulture),
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
            MaxPoints = 2
        };
        await _activityFacadeSUT.SaveAsync(activity1);
        
        var activity2 = new ActivityDetailModel
        {
            Description = "Test activity2",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = DateTime.ParseExact("10-05-2023", "dd-MM-yyyy", CultureInfo.InvariantCulture),
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
            MaxPoints = 5
        };
        await _activityFacadeSUT.SaveAsync(activity2);
        
        var activities = await _activityFacadeSUT
            .GetFromCourseAsync(CourseSeeds.CourseICS.Id, 
                startTime:DateTime.ParseExact("08-05-2023", "dd-MM-yyyy", CultureInfo.InvariantCulture));
        Assert.Single(activities);
        Assert.Equal(5, activities.ToList()[0].MaxPoints);
    }
    
    [Fact]
    public async Task GetAll_FilteredByFinishDateInGivenCourse()
    {
        var activity1 = new ActivityDetailModel
        {
            Description = "Test activity1",
            CourseId = CourseSeeds.CourseIpk.Id,
            FinishTime = DateTime.ParseExact("05-05-2023", "dd-MM-yyyy", CultureInfo.InvariantCulture),
            StartTime = default,
            ActivityType = ActivityType.Exercise,
            MaxPoints = 2
        };
        await _activityFacadeSUT.SaveAsync(activity1);
        
        var activity2 = new ActivityDetailModel
        {
            Description = "Test activity2",
            CourseId = CourseSeeds.CourseIpk.Id,
            FinishTime = DateTime.ParseExact("10-05-2023", "dd-MM-yyyy", CultureInfo.InvariantCulture),
            StartTime = default,
            ActivityType = ActivityType.Exercise,
            MaxPoints = 5
        };
        await _activityFacadeSUT.SaveAsync(activity2);
        
        var activities = await _activityFacadeSUT
            .GetFromCourseAsync(CourseSeeds.CourseIpk.Id, 
                finishTime:DateTime.ParseExact("08-05-2023", "dd-MM-yyyy", CultureInfo.InvariantCulture));
        Assert.Single(activities);
        Assert.Equal(2, activities.ToList()[0].MaxPoints);
    }

    [Fact]
    public async Task GetAll_SortedByCourseAbbreviation()
    {
        var activityIcs = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        var activityIds = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseDatabase.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        await _activityFacadeSUT.SaveAsync(activityIcs);
        await _activityFacadeSUT.SaveAsync(activityIds);
        
        var activities = await _activityFacadeSUT.GetAsync(orderQuery:"course_abbreviation");
        //skip entries without course abbreviation
        Assert.Equal("ICS", activities
            .Where(a => a.CourseAbbreviation != "")
            .ToList()[0].CourseAbbreviation);
    }
    
    [Fact]
    public async Task GetAll_SortedByCourseAbbreviationDescending()
    {
        var activityIcs = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseICS.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        var activityIds = new ActivityDetailModel
        {
            Description = "Test activity",
            CourseId = CourseSeeds.CourseDatabase.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        
        await _activityFacadeSUT.SaveAsync(activityIcs);
        await _activityFacadeSUT.SaveAsync(activityIds);
        
        var activities = await _activityFacadeSUT.GetAsync(orderQuery:"course_abbreviation", isAscending:false);
        //skip entries without course abbreviation
        Assert.Equal("IDS", activities
            .Where(a => a.CourseAbbreviation != "")
            .ToList()[0].CourseAbbreviation);
    }
    
    [Fact]
    public async Task GetAll_SortedByMaxPoints()
    {
        var activities = await _activityFacadeSUT.GetAsync(orderQuery:"max_points");
        DeepAssert.Equal(ActivityModelMapper
            .MapToListModel(ActivitySeeds.ActivityExercise), activities.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByMaxPointsDescending()
    {
        var activities = await _activityFacadeSUT
            .GetAsync(orderQuery:"max_points", isAscending:false);
        DeepAssert.Equal(ActivityModelMapper
            .MapToListModel(ActivitySeeds.ActivityExam), activities.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByStartTime()
    {
        var activities = await _activityFacadeSUT.GetAsync(orderQuery:"start_time");
        DeepAssert.Equal(ActivityModelMapper
            .MapToListModel(ActivitySeeds.ActivityExamIds), activities.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByStartTimeDescending()
    {
        var activities = await _activityFacadeSUT
            .GetAsync(orderQuery:"start_time", isAscending:false);
        DeepAssert.Equal(ActivityModelMapper
            .MapToListModel(ActivitySeeds.ActivityExercise), activities.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByFinishTime()
    {
        var activities = await _activityFacadeSUT
            .GetAsync(orderQuery:"finish_time");
        DeepAssert.Equal(ActivityModelMapper
            .MapToListModel(ActivitySeeds.ActivityExamIds), activities.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByFinishTimeDescending()
    {
        var activities = await _activityFacadeSUT
            .GetAsync(orderQuery:"finish_time", isAscending:false);
        DeepAssert.Equal(ActivityModelMapper
            .MapToListModel(ActivitySeeds.ActivityExercise), activities.ToList()[0]);
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