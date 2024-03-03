using System.ComponentModel.DataAnnotations;

namespace InformationSystem.DAL.Entities;

public record CourseEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(50)]
    public required string Name { get; set; }   
    [MaxLength(1000)]
    public required string Description { get; set; }
    [MaxLength(5)]
    public required string Abbreviation { get; set; }

    public ICollection<StudentCourseEntity> StudentCourses { get; set; } = new List<StudentCourseEntity>();
    public ICollection<ActivityEntity> Activities { get; set; } = new List<ActivityEntity>();
}