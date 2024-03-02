namespace InformationSystem.DAL.Entities;

public class CourseEntity
{
    public required Guid Id { get; set; } = new Guid();
    public required string Name { get; set; }   
    public required string Description { get; set; }
    public required string Abbreviation { get; set; }

    public ICollection<StudentEntity> Students { get; set; } = new List<StudentEntity>();
    public ICollection<ActivityEntity> Activities { get; set; } = new List<ActivityEntity>();
}