namespace InformationSystem.DAL.Entities;

public class StudentCourseEntity : IEntity
{
    public Guid Id { get; set; }
    
    public Guid StudentId { get; set; }
    public StudentEntity Student { get; set; }
    
    public Guid CourseId { get; set; }
    public CourseEntity Course { get; set; }
}