using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class StudentModelMapper(
    CourseModelMapper courseModelMapper, 
    ActivityModelMapper activityModelMapper,
    EvaluationModelMapper evaluationModelMapper): 
    ModelMapperBase<StudentEntity, StudentDetailModel, StudentListModel>
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
            .Select(a => a.Evaluation).Where(e => e != null);
        
        var student = new StudentDetailModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Group = entity.Group,
            CurrentYear = DateTime.Now.Year - entity.StartYear,
            ImageUrl = entity.ImageUrl,
            Courses = courseModelMapper
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
            StartYear = model.CurrentYear + DateTime.Now.Year,
            ImageUrl = model.ImageUrl
        };


    // public StudentEntity MapToEntity(StudentDetailModel model, )
}