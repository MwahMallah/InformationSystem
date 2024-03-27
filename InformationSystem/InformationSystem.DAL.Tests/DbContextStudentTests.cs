using InformationSystem.Common.Tests;
using InformationSystem.DAL.Entities;
using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;

namespace InformationSystem.DAL.Tests;

public class DbContextStudentTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_StudentWithoutCourses_Persisted()
    {
        var entity = StudentSeeds.StudentWithoutCourses;
        
        InformationSystemDbContextSUT.Students.Add(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();
        
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Students
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_StudentWithCourses_Persisted()
    {
        var entity = new StudentEntity()
        {
            Id = Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
            FirstName = "Maksim",
            LastName = "Dubrovin",
            ImageUrl = null,
            Group = "2B",
            StartYear = 2022,
        };
        var course1 = CourseSeeds.EmptyCourseEntity with
        {
            Name = "Operating systems",
            Abbreviation = "IOS",
            Description = ""
        };
        var course2 = CourseSeeds.EmptyCourseEntity with
        {
            Name = "Databases",
            Abbreviation = "IDS",
            Description = ""
        };
        
        entity.Courses.Add(course1);
        entity.Courses.Add(course2);

        InformationSystemDbContextSUT.Students.Add(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();
        
        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Students.Include(s => s.Courses)
            .ThenInclude(c => c.Activities)
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    
}    