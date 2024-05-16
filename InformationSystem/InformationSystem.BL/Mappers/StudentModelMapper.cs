using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class StudentModelMapper(
    ICourseListModelMapper courseListModelMapper, 
    IActivityModelMapper activityModelMapper,
    IEvaluationModelMapper evaluationModelMapper): 
    ModelMapperBase<StudentEntity, StudentDetailModel, StudentListModel>, IStudentModelMapper
{
    public override StudentListModel MapToListModel(StudentEntity entity)
        => new StudentListModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Group = entity.Group,
            CurrentYear = DateTime.Now.Year - entity.StartYear
        };

    public override StudentDetailModel MapToDetailModel(StudentEntity? entity)
    {
        if (entity is null) return StudentDetailModel.Empty;
        
        var courses = entity.Courses ?? Enumerable.Empty<CourseEntity>();;
        var activities = courses
            .SelectMany(c => c.Activities ?? Enumerable.Empty<ActivityEntity>());
        var evaluations = activities
            .SelectMany(a => a.Evaluations).Where(e => e.StudentId == entity.Id);
        
        var student = new StudentDetailModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Group = entity.Group,
            CurrentYear = DateTime.Now.Year - entity.StartYear,
            ImageUrl = entity.ImageUrl,
            Courses = courseListModelMapper
                .MapToListModel(courses).ToObservableCollection(),
            Activities = activityModelMapper
                .MapToListModel(activities).ToObservableCollection(),
            Evaluations = evaluationModelMapper
                .MapToListModel(evaluations).ToObservableCollection()
        };

        return student;
    }

    public override StudentEntity MapToEntity(StudentDetailModel model)
        => new StudentEntity
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Group = model.Group,
            StartYear = DateTime.Now.Year - model.CurrentYear,
            ImageUrl = model.ImageUrl
        };
}