using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Mappers;

public class StudentEntityMapper : IEntityMapper<StudentEntity>
{
    public void MapToExistingEntity(StudentEntity existingEntity, StudentEntity newEntity)
    {
        existingEntity.FirstName = newEntity.FirstName;
        existingEntity.LastName  = newEntity.LastName;
        existingEntity.Group = newEntity.Group;
        existingEntity.StartYear = newEntity.StartYear;
        existingEntity.Courses = newEntity.Courses;
        existingEntity.ImageUrl = newEntity.ImageUrl;
    }
}