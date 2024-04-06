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
        Id = Guid.NewGuid(),
        Name = "Seminar C#",
        Description = "C# course",
        Abbreviation = "ICS"
    };
}