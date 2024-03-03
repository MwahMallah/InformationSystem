using InformationSystem.DAL.Entities;

namespace InformationSystem.Common.Tests.Seeds;

public static class CourseSeeds
{
    public static readonly CourseEntity EmptyCourseEntity = new()
    {
        Id = default, 
        Name = default!,
        Description = default!,
        Abbreviation = default!,
        StudentCourses = default!,
        Activities = default!
    };
}