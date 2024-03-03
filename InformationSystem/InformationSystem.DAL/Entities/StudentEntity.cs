using System.ComponentModel.DataAnnotations;

namespace InformationSystem.DAL.Entities;

public class StudentEntity 
{
    public Guid Id { get; set; } = Guid.NewGuid(); 
    [MaxLength(50)]
    public required string FirstName { get; set; }
    [MaxLength(50)]
    public required string LastName { get; set; }
    public string? ImageUrl { get; set; }
    [MaxLength(5)]
    public required string Group { get; set; }
    public int CurrentYear { get; set; } 

    public Guid CourseId { get; set; }
    public ICollection<StudentCourseEntity> StudentCourses { get; init; } = new List<StudentCourseEntity>();
}