using System.Collections.ObjectModel;
using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Tests;
using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;

namespace InformationSystem.BL.Tests;

public class StudentFacadeTests: FacadeTestBase
{
    private readonly StudentFacade _studentFacadeSUT;
    
    public StudentFacadeTests(ITestOutputHelper output) : base(output)
    {
        _studentFacadeSUT = new StudentFacade(UnitOfWorkFactory, StudentModelMapper);
    }
    
    [Fact]
    public async Task AddStudentWithoutCourses()
    {
        var student = new StudentDetailModel
        {
            Id = Guid.NewGuid(), 
            FirstName = "Maksim",
            LastName = "Dubrovin",
            Group = "123",
            CurrentYear = 0,
        };

        // Act: Attempt to save the new activity
        student = await _studentFacadeSUT.SaveAsync(student);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var studentFromDb = await dbxAssert.Students.SingleAsync(i => i.Id == student.Id);
        var mapped = StudentModelMapper.MapToDetailModel(studentFromDb);
        DeepAssert.Equal(student, mapped);
    }

    [Fact]
    public async Task AddStudentWithCourses()
    {
        var student = new StudentDetailModel
        {
            Id = Guid.NewGuid(), 
            FirstName = "Maksim",
            LastName = "Dubrovin",
            Group = "123",
            CurrentYear = 0,
            Courses = new ObservableCollection<CourseListModel>
            {
                CourseModelMapper.MapToListModel(CourseSeeds.CourseDatabase)
            }
        }; 
        
        student = await _studentFacadeSUT.SaveAsync(student);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var studentFromDb = await dbxAssert.Students
            .Include(s=>s.Courses)
            .SingleAsync(i => i.Id == student.Id);
        
        var mapped = StudentModelMapper.MapToDetailModel(studentFromDb);
        DeepAssert.Equal(student, mapped);
    }
    
    [Fact]
    public async Task AddStudentWithMultipleCourses()
    {
        var student = new StudentDetailModel
        {
            Id = Guid.NewGuid(), 
            FirstName = "Maksim",
            LastName = "Dubrovin",
            Group = "123",
            CurrentYear = 0,
            Courses = new ObservableCollection<CourseListModel>
            {
                CourseModelMapper.MapToListModel(CourseSeeds.CourseDatabase),
                CourseModelMapper.MapToListModel(CourseSeeds.CourseICS)
            }
        }; 
        
        student = await _studentFacadeSUT.SaveAsync(student);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var studentFromDb = await dbxAssert.Students
            .Include(s=>s.Courses)
            .SingleAsync(i => i.Id == student.Id);
        
        var mapped = StudentModelMapper.MapToDetailModel(studentFromDb);
        DeepAssert.Equal(student, mapped);
    }
    
    [Fact]
    public async Task GetAll_StudentFindWithGivenId()
    {
        var students = await _studentFacadeSUT.GetAsync();
        var ilya = students.SingleOrDefault(s => s.Id == StudentSeeds.StudentIlya.Id);
        DeepAssert.Equal(ilya, StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya));
    }


    [Fact]
    public async Task GetById_StudentWithGivenId()
    {
        var ilya = await _studentFacadeSUT.GetAsync(StudentSeeds.StudentIlya.Id);
        DeepAssert.Equal(ilya, StudentModelMapper.MapToDetailModel(StudentSeeds.StudentIlya));
    }

    [Fact]
    public async Task GetById_NonExistentStudent()
    {
        var nonExistentStudent = await _studentFacadeSUT.GetAsync(StudentSeeds.EmptyStudentEntity.Id);
        Assert.Null(nonExistentStudent);
    }

    [Fact]
    public async Task Update_StudentWithoutCourses_IlyaUpdated()
    {
        var updatedIlya = new StudentDetailModel
        {
            Id = StudentSeeds.StudentIlya.Id,
            FirstName = StudentSeeds.StudentIlya.FirstName,
            LastName = StudentSeeds.StudentIlya.LastName,
            Group = StudentSeeds.StudentIlya.Group
        };

        updatedIlya.LastName = "Volk";
        updatedIlya.FirstName = "Iluha";
        
        await _studentFacadeSUT.SaveAsync(updatedIlya);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var dbStudent = await dbxAssert.Students.SingleAsync(s => s.Id == updatedIlya.Id);
        DeepAssert.Equal(updatedIlya, StudentModelMapper.MapToDetailModel(dbStudent));
    }

    [Fact]
    public async Task Update_StudentWithoutCourses_CourseAdded()
    {
        var updatedIlya = new StudentDetailModel
        {
            Id = StudentSeeds.StudentIlya.Id,
            FirstName = StudentSeeds.StudentIlya.FirstName,
            LastName = StudentSeeds.StudentIlya.LastName,
            Group = StudentSeeds.StudentIlya.Group
        };

        updatedIlya.Courses = new ObservableCollection<CourseListModel>
        {
            CourseModelMapper.MapToListModel(CourseSeeds.CourseDatabase)
        };
        
        await _studentFacadeSUT.SaveAsync(updatedIlya);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var dbStudent = await dbxAssert.Students
            .Include(s=>s.Courses)
            .SingleAsync(s => s.Id == updatedIlya.Id);
        DeepAssert.Equal(updatedIlya, StudentModelMapper.MapToDetailModel(dbStudent));
    }
    
    [Fact]
    public async Task Update_StudentWithoutCourses_AddCourse_NonExistentCourseThrows()
    {
        var updatedIlya = new StudentDetailModel
        {
            Id = StudentSeeds.StudentIlya.Id,
            FirstName = StudentSeeds.StudentIlya.FirstName,
            LastName = StudentSeeds.StudentIlya.LastName,
            Group = StudentSeeds.StudentIlya.Group
        };

        updatedIlya.Courses = new ObservableCollection<CourseListModel>
        {
            CourseModelMapper.MapToListModel(CourseSeeds.EmptyCourseEntity)
        };
        
        
        await Assert.ThrowsAsync<InvalidOperationException>(async()=>
                await _studentFacadeSUT.SaveAsync(updatedIlya)
        );
    }
    
    [Fact]
    public async Task Update_StudentWithCourses_CoursesDeleted()
    {
        var student = new StudentDetailModel
        {
            Id = Guid.NewGuid(), 
            FirstName = "Maksim",
            LastName = "Dubrovin",
            Group = "123",
            CurrentYear = 0,
            Courses = new ObservableCollection<CourseListModel>
            {
                CourseModelMapper.MapToListModel(CourseSeeds.CourseDatabase),
                CourseModelMapper.MapToListModel(CourseSeeds.CourseICS)
            }
        }; 
        
        student = await _studentFacadeSUT.SaveAsync(student);

        var updatedStudent = new StudentDetailModel
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Group = student.Group,
            CurrentYear = student.CurrentYear,
            Courses = new ObservableCollection<CourseListModel>()
        };
        
        await _studentFacadeSUT.SaveAsync(updatedStudent);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var dbStudent = await dbxAssert.Students
            .Include(s=>s.Courses)
            .SingleAsync(s => s.Id == updatedStudent.Id);
        DeepAssert.Equal(updatedStudent, StudentModelMapper.MapToDetailModel(dbStudent));
    }
    
    [Fact]
    public async Task Update_StudentWithCourses_CoursesUpdated()
    {
        var student = new StudentDetailModel
        {
            Id = Guid.NewGuid(), 
            FirstName = "Maksim",
            LastName = "Dubrovin",
            Group = "123",
            CurrentYear = 0,
            Courses = new ObservableCollection<CourseListModel>
            {
                CourseModelMapper.MapToListModel(CourseSeeds.CourseDatabase)
            }
        }; 
        
        student = await _studentFacadeSUT.SaveAsync(student);

        var updatedStudent = new StudentDetailModel
        {
            Id = student.Id,
            FirstName = "Maksim",
            LastName = "Dubrovin",
            Group = "123",
            CurrentYear = 0,
            Courses = new ObservableCollection<CourseListModel>
            {
                CourseModelMapper.MapToListModel(CourseSeeds.CourseICS)
            }
        };
        
        await _studentFacadeSUT.SaveAsync(updatedStudent);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var dbStudent = await dbxAssert.Students
            .Include(s=>s.Courses)
            .SingleAsync(s => s.Id == updatedStudent.Id);
        DeepAssert.Equal(updatedStudent, StudentModelMapper.MapToDetailModel(dbStudent));
    }

    [Fact]
    public async Task DeleteById_StudentIlyaDeleted()
    {
        await _studentFacadeSUT.DeleteAsync(StudentSeeds.StudentIlya.Id);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var found = await dbxAssert.Students.AnyAsync(s => s.Id == StudentSeeds.StudentIlya.Id);
        Assert.False(found);
    }

    [Fact]
    public async Task DeleteById_NonExistentStudentThrows()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async()=>
            await _studentFacadeSUT.DeleteAsync(StudentSeeds.EmptyStudentEntity.Id)
            );
    }

}