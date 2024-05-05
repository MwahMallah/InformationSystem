using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class CourseSeeds
{
    public static readonly CourseEntity EmptyCourseEntity = new()
    {
        Id = Guid.Empty, 
        Name = default!,
        Description = default!,
        Abbreviation = default!
    };

    public static readonly CourseEntity CourseICS = EmptyCourseEntity with
    {
        Id = Guid.Parse("97B7F7B6-5c58-43B3-B8C0-B5FCFFF6DC2E"),
        Name = "Seminar C#",
        Description = "C# course",
        Abbreviation = "ICS",
        Credits = 5,
        MaxStudents = 256
    };

    public static readonly CourseEntity CourseDatabase = EmptyCourseEntity with
    {
        Id = Guid.Parse("53B6A7B6-5c58-43B3-C9B0-B5FCFFF6DC2E"),
        Name = "Databases",
        Description = "Database course",
        Abbreviation = "IDS", 
        Credits = 10,
        MaxStudents = 512
    };
    
    public static readonly CourseEntity CourseIpk = EmptyCourseEntity with
    {
        Id = Guid.Parse("721FED26-5c58-43B3-C9B0-B5FCFFF6DC2E"),
        Name = "Computer Networking",
        Description = "Course about computer networks",
        Abbreviation = "IPK",
        Credits = 2,
        MaxStudents = 1024
    };
    
    public static readonly CourseEntity CourseIpp = EmptyCourseEntity with
    {
        Id = Guid.Parse("1FEDBC66-5cF8-4DB3-A9B0-A5FAFFC6DC2E"),
        Name = "OOP programming",
        Description = "Introduction to OOP programming",
        Abbreviation = "IPP",
        Credits = 7,
        MaxStudents = 420
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseEntity>()
            .HasData(
                CourseICS,
                CourseDatabase,
                CourseIpk,
                CourseIpp
            );
    }
}