using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class ActivityModelMapper: 
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
                Points = entity?.Evaluation?.Points ?? 0
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
                Description = entity.Description,
                Points = entity.Evaluation?.Points ?? 0
            };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
        => new ActivityEntity
        {
            Id = model.Id,
            StartTime = model.StartTime,
            FinishTime = model.FinishTime,
            ActivityType = model.ActivityType,
            Description = model.Description,
            CourseId = model.CourseId
        };
    
}