using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Mappers;

public class EvaluationEntityMapper : IEntityMapper<EvaluationEntity>
{
    public void MapToExistingEntity(EvaluationEntity existingEntity, EvaluationEntity newEntity)
    {
        existingEntity.Points = newEntity.Points;
        existingEntity.Description = newEntity.Description;
        existingEntity.StudentId = newEntity.StudentId;
        existingEntity.ActivityId = newEntity.ActivityId;
    }
}