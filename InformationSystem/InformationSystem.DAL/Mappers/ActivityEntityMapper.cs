using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Mappers;

public class ActivityEntityMapper: IEntityMapper<ActivityEntity>
{
    public void MapToExistingEntity(ActivityEntity existingEntity, ActivityEntity newEntity)
    {
        existingEntity.StartTime = newEntity.StartTime;
        existingEntity.FinishTime = newEntity.FinishTime;
        existingEntity.ActivityType = newEntity.ActivityType;
        existingEntity.RoomType = newEntity.RoomType;
        existingEntity.Description = newEntity.Description;
        existingEntity.CourseId = newEntity.CourseId;
        existingEntity.EvaluationId = newEntity.EvaluationId;
    }
}