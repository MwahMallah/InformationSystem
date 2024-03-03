using System.ComponentModel.DataAnnotations;

namespace InformationSystem.DAL.Entities;

public class CourseEntity : IEntity
{
    public required Guid Id { get; set; } = new Guid();
    [MaxLength(50)]
    public required string Name { get; set; }   
    [MaxLength(1000)]
    public required string Description { get; set; }
    [MaxLength(5)]
    public required string Abbreviation { get; set; }

    public ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();
    public ICollection<ActivityEntity> Activities { get; set; } = new List<ActivityEntity>();
}