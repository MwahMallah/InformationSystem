using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Tests;
using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace InformationSystem.BL.Tests;

public class CourseFacadeTests: FacadeTestBase
{
    private readonly CourseFacade _courseFacadeSUT;
    
    public CourseFacadeTests(ITestOutputHelper output) : base(output)
    {
        _courseFacadeSUT = new CourseFacade(UnitOfWorkFactory, CourseModelMapper);
    }
    
    [Fact]
    public async Task AddCourseWithoutStudents()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPK",
            Name = "Computer Networks",
            MaxStudents = 200,
            Credits = 5,
        };

        // Act: Attempt to save the new activity
        course = await _courseFacadeSUT.SaveAsync(course);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var courseFromDb = await dbxAssert.Courses.SingleAsync(i => i.Id == course.Id);
        var mapped = CourseModelMapper.MapToDetailModel(courseFromDb);
        DeepAssert.Equal(course, mapped);
    }
    
    [Fact]
    public async Task GetAll_CourseFindWithGivenId()
    {
        var courses = await _courseFacadeSUT.GetAsync();
        var ics = courses.SingleOrDefault(s => s.Id == CourseSeeds.CourseICS.Id);
        DeepAssert.Equal(ics, CourseModelMapper.MapToListModel(CourseSeeds.CourseICS));
    }
    
    [Fact]
    public async Task GetById_CourseWithGivenId()
    {
        var ids = await _courseFacadeSUT.GetAsync(CourseSeeds.CourseDatabase.Id);
        DeepAssert.Equal(ids, CourseModelMapper.MapToDetailModel(CourseSeeds.CourseDatabase));
    }
    
    [Fact]
    public async Task GetById_NonExistentCourse()
    {
        var nonExistentCourse = await _courseFacadeSUT.GetAsync(CourseSeeds.EmptyCourseEntity.Id);
        Assert.Null(nonExistentCourse);
    }
    
    
    [Fact]
    public async Task Update_CourseDatabaseUpdated()
    {
        var updatedIds = new CourseDetailModel
        {
            Id = CourseSeeds.CourseDatabase.Id,
            Abbreviation = "IDS",
            Name = "Databases",
        };

        updatedIds.Name += "Db";
        updatedIds.Description += "Very interesting course";
        
        await _courseFacadeSUT.SaveAsync(updatedIds);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var dbCourses = await dbxAssert.Courses.SingleAsync(s => s.Id == updatedIds.Id);
        DeepAssert.Equal(updatedIds, CourseModelMapper.MapToDetailModel(dbCourses));
    }

    [Fact]
    public async Task DeleteById_StudentIlyaDeleted()
    {
        await _courseFacadeSUT.DeleteAsync(CourseSeeds.CourseICS.Id);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var found = await dbxAssert.Courses.AnyAsync(s => s.Id == CourseSeeds.CourseICS.Id);
        Assert.False(found);
    }
    
    [Fact]
    public async Task DeleteById_NonExistentStudentThrows()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async()=>
            await _courseFacadeSUT.DeleteAsync(CourseSeeds.EmptyCourseEntity.Id)
        );
    }
}