using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class ActivityModelMapper(EvaluationModelMapper evaluationModelMapper): 
    ModelMapperBase<ActivityEntity, ActivityDetailModel, ActivityListModel>
{
    public override ActivityListModel MapToListModel(ActivityEntity entity) 
        => new ActivityListModel
            {
                Id = entity.Id,
                StartTime = entity.StartTime,
                FinishTime = entity.FinishTime,
                ActivityType = entity.ActivityType,
                CourseAbbreviation = entity?.Course?.Abbreviation ?? string.Empty,
                MaxPoints = entity.MaxPoints
            };
    
    public override ActivityDetailModel MapToDetailModel(ActivityEntity entity)
        => new ActivityDetailModel
            {
                Id = entity.Id,
                StartTime = entity.StartTime,
                FinishTime = entity.FinishTime,
                ActivityType = entity.ActivityType,
                CourseAbbreviation = entity?.Course?.Abbreviation ?? string.Empty,
                CourseId = entity.CourseId,
                Evaluations = evaluationModelMapper.MapToListModel(entity.Evaluations).ToObservableCollection(),
                Description = entity.Description,
                MaxPoints = entity.MaxPoints
            };
    
    public override ActivityEntity MapToEntity(ActivityDetailModel model)
        => new ActivityEntity
        {
            Id = model.Id,
            StartTime = model.StartTime,
            FinishTime = model.FinishTime,
            ActivityType = model.ActivityType,
            Description = model.Description,
            CourseId = model.CourseId,
            MaxPoints = model.MaxPoints
        };
    
}