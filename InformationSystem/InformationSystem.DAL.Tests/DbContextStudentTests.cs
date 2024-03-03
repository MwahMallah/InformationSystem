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
        //Arrange
        StudentEntity entity = StudentSeeds.StudentWithoutCourses;

        //Act
        InformationSystemDbContextSUT.Students.Add(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Students
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task AddNew_StudentWithCourses_Persisted()
    {
        StudentEntity entity = new StudentEntity()
        {
            Id = Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
            FirstName = "Maksim",
            LastName = "Dubrovin",
            ImageUrl = null,
            Group = "2B",
            CurrentYear = 2,
            // CourseId = ,
            // StudentCourses = 
        };
        CourseEntity course1 = CourseSeeds.EmptyCourseEntity with
        {
            Name = "Operating systems",
            Abbreviation = "IOS",
            Description = ""
        };
        CourseEntity course2 = CourseSeeds.EmptyCourseEntity with
        {
            Name = "Databases",
            Abbreviation = "IDS",
            Description = ""
        };
        
        //Act
        InformationSystemDbContextSUT.Students.Add(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        InformationSystemDbContextSUT.Courses.Add(course1);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        InformationSystemDbContextSUT.Courses.Add(course2);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        var sc1 = StudentCourseSeeds.EmptyStudentCourseEntity with
        {
            Student = entity, Course = course1
        };
        
        var sc2 = StudentCourseSeeds.EmptyStudentCourseEntity with
        {
            Student = entity, Course = course2
        };
        
        InformationSystemDbContextSUT.StudentsCourses.Add(sc1);
        await InformationSystemDbContextSUT.SaveChangesAsync();
        
        InformationSystemDbContextSUT.StudentsCourses.Add(sc2);
        await InformationSystemDbContextSUT.SaveChangesAsync();
        
        // entity.StudentCourses.Add(sc1);
        // await InformationSystemDbContextSUT.SaveChangesAsync();
        // entity.StudentCourses.Add(sc2);
        // await InformationSystemDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Students
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

}    