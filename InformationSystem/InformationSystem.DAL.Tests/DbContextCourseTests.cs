using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;
using InformationSystem.Common.Tests;
using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Tests
{
    public class DbContextCourseTests(ITestOutputHelper output) : DbContextTestsBase(output)
    {
        [Fact]
        public async Task AddNew_CourseWithoutStudents_Persisted()
        {
            var course = CourseSeeds.CourseEntity;

            InformationSystemDbContextSUT.Courses.Add(course);
            await InformationSystemDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbCourse = await dbx.Courses
                .SingleAsync(i => i.Id == course.Id);
            DeepAssert.Equal(course, dbCourse);
        }
        
        [Fact]
        public async Task AddNew_CourseWithStudents_Persisted()
        {
            var course = CourseSeeds.CourseEntity with
            {
                Name = "zaklady programovani",
                Description = "c lang course",
                Abbreviation = "IZP",
                Students = new List<StudentEntity> {
                    StudentSeeds.EmptyStudentEntity with
                    {
                        FirstName = "Ilya",
                        LastName = "Volkov",
                        StartYear = 2020,
                        Group = "1A"
                    },
                    StudentSeeds.EmptyStudentEntity with
                    {
                        FirstName = "Tomas",
                        LastName = "Dvorak",
                        StartYear = 2022,
                        Group = "1B"
                    }
                }
            };
            
            
            InformationSystemDbContextSUT.Courses.Add(course);
            await InformationSystemDbContextSUT.SaveChangesAsync();

            //Assert
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbCourse = await dbx.Courses
                .Include(i => i.Students)
                .SingleAsync(i => i.Id == course.Id);
            DeepAssert.Equal(course, dbCourse);
        }
        

        [Fact]
        public async Task GetAll_Courses_ContainsSeededCourse()
        {
            var courses = await InformationSystemDbContextSUT.Courses.ToListAsync();

            //Assert
            DeepAssert.Contains(CourseSeeds.CourseEntity, courses);
        }

        [Fact]
        public async Task GetById_Course()
        {
            var course = await InformationSystemDbContextSUT.Courses
                .SingleAsync(i => i.Id == CourseSeeds.CourseEntity.Id);
        
            //Assert
            DeepAssert.Equal(CourseSeeds.CourseEntity with { Students = Array.Empty<StudentEntity>() }, course);
        }
        
        [Fact]
        public async Task GetById_IncludingStudents_Recipe()
        {
            var entity = await InformationSystemDbContextSUT.Courses
                .Include(i => i.Students)
                .SingleAsync(i => i.Id == CourseSeeds.CourseEntity.Id);
        
            //Assert
            DeepAssert.Equal(CourseSeeds.CourseEntity, entity);
        }
        
        [Fact]
        public async Task Update_Course_Persisted()
        {
            //Arrange
            var baseEntity = CourseSeeds.CourseEntity;
            var entity =
                baseEntity with
                {
                    Name = baseEntity.Name + "Updated",
                    Description = baseEntity.Description + "Updated"
                };
            
            InformationSystemDbContextSUT.Courses.Update(entity);
            await InformationSystemDbContextSUT.SaveChangesAsync();
        
            //Assert
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var actualEntity = await dbx.Courses.SingleAsync(i => i.Id == entity.Id);
            DeepAssert.Equal(entity, actualEntity);
        }
        
        [Fact]
        public async Task Delete_CourseWithoutStudents_Deleted()
        {
            var baseEntity = CourseSeeds.CourseEntity;
            
            InformationSystemDbContextSUT.Courses.Remove(baseEntity);
            await InformationSystemDbContextSUT.SaveChangesAsync();
        
            //Assert
            Assert.False(await InformationSystemDbContextSUT.Courses.AnyAsync(i => i.Id == baseEntity.Id));
        }
        
        [Fact]
        public async Task DeleteById_CourseWithoutStudents_Deleted()
        {
            //Arrange
            var baseEntity = CourseSeeds.CourseWithoutStudents;
        
            //Act
            InformationSystemDbContextSUT.Remove(
                InformationSystemDbContextSUT.Courses.Single(i => i.Id == baseEntity.Id));
            await InformationSystemDbContextSUT.SaveChangesAsync();
        
            //Assert
            Assert.False(await InformationSystemDbContextSUT.Courses.AnyAsync(i => i.Id == baseEntity.Id));
        }
        
        [Fact]
        public async Task Delete_CourseWithStudents_Deleted()
        {
            //Arrange
            var baseEntity = CourseSeeds.CourseEntity with
            {
                Name = "zaklady programovani",
                Description = "c lang course",
                Abbreviation = "IZP",
                Students = new List<StudentEntity>
                {
                    StudentSeeds.EmptyStudentEntity with
                    {
                        FirstName = "Ilya",
                        LastName = "Volkov",
                        StartYear = 2020,
                        Group = "1A"
                    },
                    StudentSeeds.EmptyStudentEntity with
                    {
                        FirstName = "Tomas",
                        LastName = "Dvorak",
                        StartYear = 2022,
                        Group = "1B"
                    }
                }
            };
            
            InformationSystemDbContextSUT.Courses.Remove(baseEntity);
            await InformationSystemDbContextSUT.SaveChangesAsync();
        
            //Assert
            Assert.False(await InformationSystemDbContextSUT.Courses.AnyAsync(i => i.Id == baseEntity.Id));
            Assert.False(await InformationSystemDbContextSUT.Students
                .AnyAsync(i => baseEntity.Students.Select(student => student.Id).Contains(i.Id)));
        }
    }
}
