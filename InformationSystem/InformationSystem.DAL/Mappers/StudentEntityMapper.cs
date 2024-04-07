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
        // existingEntity.Courses = newEntity.Courses;
        
        // Handle the Courses collection more carefully
        // Remove courses not in the new entity
        foreach (var existingCourse in existingEntity.Courses.ToList())
        {
            if (!newEntity.Courses.Any(c => c.Id == existingCourse.Id))
            {
                existingEntity.Courses.Remove(existingCourse);
            }
        }
        
        // Add or update courses from the new entity
        foreach (var newCourse in newEntity.Courses)
        {
            var existingCourse = existingEntity.Courses
                .FirstOrDefault(c => c.Id == newCourse.Id);
        
            if (existingCourse == null)
            {
                // If the course is new, add it
                existingEntity.Courses.Add(newCourse);
            }
        }

    }
}