using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Tests;
using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace InformationSystem.BL.Tests;

public class CourseFacadeTests: FacadeTestBase
{
    private readonly CourseFacade _courseFacadeSUT;
    
    public CourseFacadeTests(ITestOutputHelper output) : base(output)
    {
        _courseFacadeSUT = new CourseFacade(UnitOfWorkFactory, CourseModelMapper);
    }
    
    [Fact]
    public async Task AddCourseWithoutStudents()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPA",
            Name = "Computer Networks",
            MaxStudents = 200,
            Credits = 5,
        };

        // Act: Attempt to save the new activity
        course = await _courseFacadeSUT.SaveAsync(course);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var courseFromDb = await dbxAssert.Courses.SingleAsync(i => i.Id == course.Id);
        var mapped = CourseModelMapper.MapToDetailModel(courseFromDb);
        DeepAssert.Equal(course, mapped);
    }
    
    [Fact]
    public async Task AddCourseWithStudents()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPA",
            Name = "networks",
            Students = new ObservableCollection<StudentListModel>
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentMaksim)
            },
        }; 
        
        course = await _courseFacadeSUT.SaveAsync(course);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Students)
            .SingleAsync(i => i.Id == course.Id);
        
        var mapped = CourseModelMapper.MapToDetailModel(courseFromDb);
        DeepAssert.Equal(course, mapped);
    }
    
    [Fact]
    public async Task AddCourseWithActivity()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPK",
            Name = "networks",
            Activities = new ObservableCollection<ActivityListModel>
            {
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise)
            },
        }; 
        
        course = await _courseFacadeSUT.SaveAsync(course);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Activities)
            .SingleAsync(i => i.Id == course.Id);
        
        var mapped = CourseModelMapper.MapToDetailModel(courseFromDb);
        DeepAssert.Equal(course, mapped);
    }
    
    [Fact]
    public async Task AddCourseWithMultipleActivities()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPK",
            Name = "networks",
            Activities = new ObservableCollection<ActivityListModel>
            {
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise),
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExam),
            },
        }; 
        
        course = await _courseFacadeSUT.SaveAsync(course);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Activities)
            .SingleAsync(i => i.Id == course.Id);
        
        var mapped = CourseModelMapper.MapToDetailModel(courseFromDb);
        DeepAssert.Equal(course, mapped);
    }
    
    [Fact]
    public async Task AddCourseWithMultipleStudents()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPK",
            Name = "networks",
            Students = new ObservableCollection<StudentListModel>
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentMaksim),
                StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya)
            },
        }; 
        
        course = await _courseFacadeSUT.SaveAsync(course);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Students)
            .SingleAsync(i => i.Id == course.Id);
        
        var mapped = CourseModelMapper.MapToDetailModel(courseFromDb);
        DeepAssert.Equal(course, mapped);
    }
    
    [Fact]
    public async Task GetAll_CourseFindWithGivenId()
    {
        var courses = await _courseFacadeSUT.GetAsync();
        var ics = courses.SingleOrDefault(s => s.Id == CourseSeeds.CourseICS.Id);
        DeepAssert.Equal(CourseModelMapper.MapToListModel(CourseSeeds.CourseICS), ics);
    }

    [Fact]
    public async Task GetAll_FilteredByAbbreviation()
    {
        var courses = await _courseFacadeSUT.GetAsync("IP");
        Assert.Equal(2, courses.Count());
    }
    
    [Fact]
    public async Task GetAll_FilteredByName()
    {
        var courses = await _courseFacadeSUT.GetAsync("networking");
        Assert.Single(courses);
    }

    [Fact]
    public async Task GetAll_SortedByCredits()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"credits");
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseIpk), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByCreditsDescending()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"credits", isAscending:false);
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseDatabase), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByName()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"name");
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseIpk), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByNameDescending()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"name", isAscending:false);
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseICS), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByAbbreviation()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"abbreviation");
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseICS), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByAbbreviationDescending()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"abbreviation", isAscending:false);
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseIpp), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByMaxStudents()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"max_students");
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseICS), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetAll_SortedByMaxStudentsDescending()
    {
        var courses = await _courseFacadeSUT.GetAsync(orderQuery:"max_students", isAscending:false);
        DeepAssert.Equal(CourseModelMapper
            .MapToListModel(CourseSeeds.CourseIpk), courses.ToList()[0]);
    }
    
    [Fact]
    public async Task GetById_CourseWithGivenId()
    {
        var ids = await _courseFacadeSUT.GetAsync(CourseSeeds.CourseDatabase.Id);
        DeepAssert.Equal(ids, CourseModelMapper.MapToDetailModel(CourseSeeds.CourseDatabase));
    }

    [Fact]
    public async Task GetById_CourseWithStudents()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPK",
            Name = "networks",
            Students = new ObservableCollection<StudentListModel>
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentMaksim),
                StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya)
            },
        }; 
        
        course = await _courseFacadeSUT.SaveAsync(course);
        var courseFromDb = await _courseFacadeSUT.GetAsync(course.Id);
        
        DeepAssert.Equal(course, courseFromDb);
    }
    
    [Fact]
    public async Task GetById_CourseWithActivities()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPK",
            Name = "networks",
            Activities = new ObservableCollection<ActivityListModel>
            {
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise),
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExam),
            },
        }; 
        
        course = await _courseFacadeSUT.SaveAsync(course);
        var courseFromDb = await _courseFacadeSUT.GetAsync(course.Id);
        
        DeepAssert.Equal(course, courseFromDb);
    }

    [Fact]
    public async Task GetById_CourseWithActivitiesAndStudents()
    {
        var course = new CourseDetailModel
        {
            Id = Guid.NewGuid(),
            Abbreviation = "IPK",
            Name = "networks",
            Students = new ObservableCollection<StudentListModel>
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentMaksim),
                StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya)
            },
            Activities = new ObservableCollection<ActivityListModel>
            {
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise),
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExam),
            },
        }; 
        
        course = await _courseFacadeSUT.SaveAsync(course);
        var courseFromDb = await _courseFacadeSUT.GetAsync(course.Id);
        
        DeepAssert.Equal(course, courseFromDb);
    }
    
    [Fact]
    public async Task GetById_NonExistentCourse()
    {
        var nonExistentCourse = await _courseFacadeSUT.GetAsync(CourseSeeds.EmptyCourseEntity.Id);
        Assert.Null(nonExistentCourse);
    }
    
    [Fact]
    public async Task GetById_CourseFoundAfterStudentUpdate()
    {
        var updatedCourse = new CourseDetailModel
        {
            Id = CourseSeeds.CourseDatabase.Id,
            Abbreviation = "IDS",
            Name = "Databases",
        };

        await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        updatedCourse.Students = new ObservableCollection<StudentListModel>
        {
            StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya)
        };
        
        await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        var courseFromDb = await _courseFacadeSUT.GetAsync(updatedCourse.Id);
        DeepAssert.Equal(updatedCourse, courseFromDb);
    }
    
    
    [Fact]
    public async Task Update_CourseUpdated()
    {
        var updatedCourse = new CourseDetailModel
        {
            Id = CourseSeeds.CourseDatabase.Id,
            Abbreviation = "IDS",
            Name = "Databases",
        };

        updatedCourse.Name += "Db";
        updatedCourse.Description += "Very interesting course";
        
        await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var courseFromDbs = await dbxAssert.Courses.SingleAsync(s => s.Id == updatedCourse.Id);
        DeepAssert.Equal(updatedCourse, CourseModelMapper.MapToDetailModel(courseFromDbs));
    }
    
    [Fact]
    public async Task Update_CourseWithoutStudents_StudentAdded()
    {
        var updatedCourse = new CourseDetailModel
        {
            Id = CourseSeeds.CourseDatabase.Id,
            Abbreviation = "IDS",
            Name = "Databases",
        };

        await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        updatedCourse.Students = new ObservableCollection<StudentListModel>
        {
            StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya)
        };
        
        updatedCourse = await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Students)
            .SingleAsync(c => c.Id == updatedCourse.Id);

        Assert.Contains(StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya),
                            CourseModelMapper.MapToDetailModel(courseFromDb).Students);
        DeepAssert.Equal(updatedCourse, CourseModelMapper.MapToDetailModel(courseFromDb));
    }
    
    [Fact]
    public async Task Update_CourseWithoutActivities_ActivityAdded()
    {
        var updatedCourse = new CourseDetailModel
        {
            Id = CourseSeeds.CourseDatabase.Id,
            Abbreviation = "IDS",
            Name = "Databases",
        };

        updatedCourse.Activities = new ObservableCollection<ActivityListModel>
        {
            ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise)
        };
        
        await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Activities)
            .SingleAsync(c => c.Id == updatedCourse.Id);

        var activity = ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise);
        var activities = CourseModelMapper.MapToDetailModel(courseFromDb).Activities;

        var model = CourseModelMapper.MapToDetailModel(courseFromDb);
        Assert.Contains(activity.Id, activities.Select(a=>a.Id));
    }
    
    [Fact]
    public async Task Update_CourseWithStudents_StudentDeleted()
    {
        var course = new CourseDetailModel
        {
            Abbreviation = "IPK",
            Name = "Computer networks",
            Students = new ObservableCollection<StudentListModel>()
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya)
            }
        };
        
        course = await _courseFacadeSUT.SaveAsync(course);

        var updatedCourse = new CourseDetailModel
        {
            Id = course.Id,
            Students = new ObservableCollection<StudentListModel>(),
            Abbreviation = course.Abbreviation,
            Name = course.Name
        };
        
        await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Students)
            .SingleAsync(c => c.Id == updatedCourse.Id);
        
        DeepAssert.Equal(updatedCourse, CourseModelMapper.MapToDetailModel(courseFromDb));
    }

    [Fact]
    public async Task Update_CourseWithActivity_ActivityDeleted()
    {
        var course = new CourseDetailModel
        {
            Id = CourseSeeds.CourseDatabase.Id,
            Abbreviation = "IDS",
            Name = "Databases",
            Activities = new ObservableCollection<ActivityListModel>
            {
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise)
            }
        };
        
        await _courseFacadeSUT.SaveAsync(course);
        
        var updatedCourse = new CourseDetailModel  
        {
            Id = course.Id,
            Activities = new ObservableCollection<ActivityListModel>(),
            Abbreviation = course.Abbreviation,
            Name = course.Name
        };
        
        updatedCourse = await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Activities)
            .SingleAsync(c => c.Id == updatedCourse.Id);

        var activity = ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise);
        var activities = CourseModelMapper.MapToDetailModel(courseFromDb).Activities;

        Assert.DoesNotContain(activity.Id, activities.Select(a=>a.Id));
    }
    
    [Fact]
    public async Task Update_CourseWithStudents_StudentUpdated()
    {
        var course = new CourseDetailModel
        {
            Abbreviation = "IPK",
            Name = "Computer networks",
            Students = new ObservableCollection<StudentListModel>()
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya)
            }
        };
        
        course = await _courseFacadeSUT.SaveAsync(course);

        var updatedCourse = new CourseDetailModel
        {
            Id = course.Id,
            Abbreviation = course.Abbreviation,
            Name = course.Name,
            Students = new ObservableCollection<StudentListModel>()
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentMaksim)
            }
        };
        
        await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Students)
            .SingleAsync(c => c.Id == updatedCourse.Id);
        
        DeepAssert.Equal(updatedCourse, CourseModelMapper.MapToDetailModel(courseFromDb));
    }
    
    [Fact]
    public async Task Update_CourseWithActivities_ActivityUpdated()
    {
        var course = new CourseDetailModel
        {
            Id = CourseSeeds.CourseDatabase.Id,
            Abbreviation = "IDS",
            Name = "Databases",
            Activities = new ObservableCollection<ActivityListModel>
            {
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise)
            }
        };
        
        await _courseFacadeSUT.SaveAsync(course);
        
        var updatedCourse = new CourseDetailModel  
        {
            Id = course.Id,
            Abbreviation = course.Abbreviation,
            Name = course.Name,
            Activities = new ObservableCollection<ActivityListModel>()
            {
                ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExam)
            }
        };
        
        updatedCourse = await _courseFacadeSUT.SaveAsync(updatedCourse);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var courseFromDb = await dbxAssert.Courses
            .Include(c=>c.Activities)
            .SingleAsync(c => c.Id == updatedCourse.Id);

        var activityExercise = ActivityModelMapper.MapToListModel(ActivitySeeds.ActivityExercise);
        var activitiesFromDb = CourseModelMapper.MapToDetailModel(courseFromDb).Activities;

        Assert.DoesNotContain(activityExercise, activitiesFromDb);
    }
    
    [Fact]
    public async Task Update_CourseWithStudents_StudentDeletedOutside()
    {
        var course = new CourseDetailModel
        {
            Abbreviation = "IPK",
            Name = "Computer networks",
            Students = new ObservableCollection<StudentListModel>()
            {
                StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya),
                StudentModelMapper.MapToListModel(StudentSeeds.StudentMaksim)
            }
        };
        
        course = await _courseFacadeSUT.SaveAsync(course);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        dbxAssert.Students.Remove(StudentSeeds.StudentIlya);
        await dbxAssert.SaveChangesAsync();
        
        var courseFromDb = await _courseFacadeSUT.GetAsync(course.Id);
        Assert.Contains(StudentModelMapper.MapToListModel(StudentSeeds.StudentMaksim), courseFromDb.Students);
        Assert.DoesNotContain(StudentModelMapper.MapToListModel(StudentSeeds.StudentIlya), courseFromDb.Students);
    }


    [Fact]
    public async Task DeleteById_CourseICSDeleted()
    {
        await _courseFacadeSUT.DeleteAsync(CourseSeeds.CourseICS.Id);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var found = await dbxAssert.Courses.AnyAsync(s => s.Id == CourseSeeds.CourseICS.Id);
        Assert.False(found);
    }
    
    [Fact]
    public async Task DeleteById_NonExistentCourseThrows()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async()=>
            await _courseFacadeSUT.DeleteAsync(CourseSeeds.EmptyCourseEntity.Id)
        );
    }
}