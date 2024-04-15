using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;
using InformationSystem.Common.Tests;

namespace InformationSystem.DAL.Tests
{
    public class DbContextCourseTests(ITestOutputHelper output) : DbContextTestsBase(output)
    {
        [Fact]
        public async Task AddNew_CourseWithoutStudents_Persisted()
        {
            var course = CourseSeeds.CourseICS;

            InformationSystemDbContextSUT.Courses.Add(course);
            await InformationSystemDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbCourse = await dbx.Courses
                .SingleAsync(i => i.Id == course.Id);
            DeepAssert.Equal(course, dbCourse);
        }
    }
}
