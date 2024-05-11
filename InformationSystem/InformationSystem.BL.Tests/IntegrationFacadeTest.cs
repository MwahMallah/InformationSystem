using System.Collections.ObjectModel;
using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Enums;
using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace InformationSystem.BL.Tests;

public class IntegrationFacadeTest: FacadeTestBase
{
    private readonly CourseFacade _courseFacadeSUT;
    private readonly StudentFacade _studentFacadeSUT;
    private readonly ActivityFacade _activityFacadeSUT;
    private readonly EvaluationFacade _evaluationFacadeSUT;
    
    public IntegrationFacadeTest(ITestOutputHelper output) : base(output)
    {
        _courseFacadeSUT = new CourseFacade(UnitOfWorkFactory, CourseModelMapper);
        _studentFacadeSUT = new StudentFacade(UnitOfWorkFactory, StudentModelMapper);
        _activityFacadeSUT = new ActivityFacade(UnitOfWorkFactory, ActivityModelMapper);
        _evaluationFacadeSUT = new EvaluationFacade(UnitOfWorkFactory, EvaluationModelMapper);
    }
    
    [Fact]
    public async Task Student_AddCourses_AddActivitiesToCourse_StudentGetsActivities()
    {
        //create new student with database course
        var newStudent = new StudentDetailModel
        {
            FirstName = "Nastya",
            LastName = "Mironova",
            Group = "123",
            Courses = new ObservableCollection<CourseListModel>()
            {
                CourseModelMapper.MapToListModel(CourseSeeds.CourseDatabase)
            }
        };

        newStudent = await _studentFacadeSUT.SaveAsync(newStudent);
        
        //make new activities for database course
        var activityModel = new ActivityDetailModel
        {
            StartTime = DateTime.Today,
            FinishTime = DateTime.Today,
            ActivityType = ActivityType.Exercise,
            CourseId = CourseSeeds.CourseDatabase.Id
        };
        
        activityModel = await _activityFacadeSUT.SaveAsync(activityModel);
        var activityModel2 = new ActivityDetailModel
        {
            StartTime = DateTime.Today.AddDays(1),
            FinishTime = DateTime.Today.AddDays(2),
            ActivityType = ActivityType.Exam,
            CourseId = CourseSeeds.CourseDatabase.Id
        };
        activityModel2 = await _activityFacadeSUT.SaveAsync(activityModel2);
        
        //student should now be able to retrieve this activity
        var studentWithActivities = await _studentFacadeSUT.GetAsync(newStudent.Id);
        
        Assert.Contains(activityModel.Id, studentWithActivities.Activities.Select(a => a.Id));
        Assert.Contains(activityModel2.Id, studentWithActivities.Activities.Select(a => a.Id));
    }


    [Fact]
    public async Task Activity_AddsEvaluation()
    {
        var newActivity = new ActivityDetailModel
        {
            CourseId = CourseSeeds.CourseDatabase.Id,
            StartTime = default,
            FinishTime = default,
            ActivityType = ActivityType.Exercise,
        };
        newActivity = await _activityFacadeSUT.SaveAsync(newActivity);
        
        var newEvaluation = new EvaluationDetailModel()
        {
            Points = 10,
            Description = "Very good",
            ActivityId = newActivity.Id,
            StudentId = StudentSeeds.StudentIlya.Id
        };

        newEvaluation = await _evaluationFacadeSUT.SaveAsync(newEvaluation);

        var activityFromDb = await _activityFacadeSUT.GetAsync(newActivity.Id);
        Assert.Contains(newEvaluation.Id, activityFromDb.Evaluations.Select(e => e.Id));
        Assert.Equal("Ilya Volkov", activityFromDb.Evaluations[0].StudentFullName);
    }
    
    [Fact]
    public async Task Student_addsCourseWithActivitiesGetEvaluation()
    {
        var newStudent = new StudentDetailModel
        {
            FirstName = "Nastya",
            LastName = "Mironova",
            Group = "123",
            Courses = new ObservableCollection<CourseListModel>()
            {
                CourseModelMapper.MapToListModel(CourseSeeds.CourseDatabase)
            }
        };

        newStudent = await _studentFacadeSUT.SaveAsync(newStudent);
        
        var newDatabaseCourseActivity = new ActivityDetailModel
        {
            StartTime = DateTime.Today,
            FinishTime = DateTime.Today,
            ActivityType = ActivityType.Exercise,
            CourseId = CourseSeeds.CourseDatabase.Id
        };
        
        newDatabaseCourseActivity = await _activityFacadeSUT.SaveAsync(newDatabaseCourseActivity);
        var newDatabaseCourseActivity2 = new ActivityDetailModel
        {
            StartTime = DateTime.Today.AddDays(1),
            FinishTime = DateTime.Today.AddDays(2),
            ActivityType = ActivityType.Exam,
            CourseId = CourseSeeds.CourseDatabase.Id
        };
        newDatabaseCourseActivity2 = await _activityFacadeSUT.SaveAsync(newDatabaseCourseActivity2);

        var newEvaluation = new EvaluationDetailModel()
        {
            StudentId = newStudent.Id,
            ActivityId = newDatabaseCourseActivity.Id,
            Points = 2,
        };

        newEvaluation = await _evaluationFacadeSUT.SaveAsync(newEvaluation);

        var studentFromDb = await _studentFacadeSUT.GetAsync(newStudent.Id);
        Assert.Equal(2, studentFromDb.Activities.Count);
        Assert.Single(studentFromDb.Evaluations);
    }
}