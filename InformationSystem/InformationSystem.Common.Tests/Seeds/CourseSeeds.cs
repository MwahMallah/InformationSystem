using InformationSystem.DAL.Entities;

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
        Id = Guid.Parse("97B7F7B6-5c58-43B3-B8C0-B5FCFFF6DC2E"),
        Name = "Seminar C#",
        Description = "C# course",
        Abbreviation = "ICS"
    };

    public static readonly CourseEntity CourseWithoutStudents = CourseEntity with
    {
        Id = Guid.Parse("53B6A7B6-5c58-43B3-C9B0-B5FCFFF6DC2E")
    };
}