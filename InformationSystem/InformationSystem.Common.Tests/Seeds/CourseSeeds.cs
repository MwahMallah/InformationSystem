using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class CourseSeeds
{
    public static readonly CourseEntity EmptyCourseEntity = new()
    {
        Id = default, 
        Name = default!,
        Description = default!,
        Abbreviation = default!
    };

    public static readonly CourseEntity CourseEntity = new()
    {
        Id = Guid.NewGuid(),
        Name = "Seminar C#",
        Description = "C# course",
        Abbreviation = "ICS"
    };

    public static readonly CourseEntity CourseWithoutStudents = CourseEntity with
    {
        Id = Guid.NewGuid()
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<CourseEntity>().HasData(
            CourseEntity,
            EmptyCourseEntity,
            CourseWithoutStudents with{Students = Array.Empty<StudentEntity>()}
            );
}