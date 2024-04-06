using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class ActivityModelMapper: 
    ModelMapperBase<ActivityEntity, ActivityDetailModel, ActivityListModel>
{
    public override ActivityListModel MapToListModel(ActivityEntity entity) 
        => entity?.Course is null
            ? ActivityListModel.Empty 
            : new ActivityListModel
            {
                Id = entity.Id,
                StartTime = entity.StartTime,
                FinishTime = entity.FinishTime,
                ActivityType = entity.ActivityType,
                CourseAbbreviation = entity.Course.Abbreviation,
                Points = entity.Evaluation?.Points ?? 0
            };
    
    public override ActivityDetailModel MapToDetailModel(ActivityEntity entity)
        => entity?.Course is null
            ? ActivityDetailModel.Empty
            : new ActivityDetailModel
            {
                Id = entity.Id,
                StartTime = entity.StartTime,
                FinishTime = entity.FinishTime,
                ActivityType = entity.ActivityType,
                CourseAbbreviation = entity.Course.Abbreviation,
                Description = entity.Description,
                Points = entity.Evaluation?.Points ?? 0
            };

    public override ActivityEntity MapToEntity(ActivityDetailModel model)
        => throw new NotImplementedException("Use other method");

    public ActivityEntity MapToEntity(ActivityDetailModel model, Guid courseId)
        => new ActivityEntity
        {
            Id = model.Id,
            Description = model.Description,
            CourseId = courseId,
            Course = null!,
            Evaluation = null!,
            StartTime = model.StartTime,
            FinishTime = model.FinishTime,
            ActivityType = model.ActivityType,
        };
    
    public ActivityEntity MapToEntity(ActivityListModel model, Guid courseId)
        => new ActivityEntity
        {
            Id = model.Id,
            CourseId = courseId,
            Course = null!,
            Evaluation = null!,
            StartTime = model.StartTime,
            FinishTime = model.FinishTime,
            ActivityType = model.ActivityType,
        };
}