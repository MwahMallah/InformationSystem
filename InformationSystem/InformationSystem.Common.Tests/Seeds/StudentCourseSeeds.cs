using InformationSystem.DAL.Entities;

namespace InformationSystem.Common.Tests.Seeds;

public static class StudentCourseSeeds
{
    public static readonly StudentCourseEntity EmptyStudentCourseEntity = new()
    {
        Id = default, 
        StudentId = default,
        Student = default!,
        CourseId = default,
        Course = default!,
    };
}