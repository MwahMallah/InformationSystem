using InformationSystem.Common.Enums;

namespace InformationSystem.BL.Models;

public record EvaluationListModel : BaseModel
{
    public int Points { get; set; }
    public Guid? StudentId { get; set; }
    public string? StudentFullName { get; set; }
    public string? CourseAbbreviation { get; set; }
    public Guid? ActivityId { get; set; }
    public ActivityType? ActivityType { get; set; }
    
    public static EvaluationListModel Empty => new()
    {
        Id = Guid.Empty,
        StudentId = Guid.Empty,
        ActivityId = Guid.Empty
    };
}