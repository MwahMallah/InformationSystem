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

    public static readonly CourseEntity CourseICS = new()
    {
        Id = Guid.Parse("97B7F7B6-5c58-43B3-B8C0-B5FCFFF6DC2E"),
        Name = "Seminar C#",
        Description = "C# course",
        Abbreviation = "ICS"
    };

    public static readonly CourseEntity CourseDatabase = CourseICS with
    {
        Id = Guid.Parse("53B6A7B6-5c58-43B3-C9B0-B5FCFFF6DC2E"),
        Name = "Databases",
        Description = "Database course",
        Abbreviation = "IDS"
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CourseEntity>()
            .HasData(
                CourseICS,
                CourseDatabase
            );
    }
}