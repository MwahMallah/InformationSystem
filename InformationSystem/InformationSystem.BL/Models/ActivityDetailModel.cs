using InformationSystem.Common.Enums;

namespace InformationSystem.BL.Models;

public record ActivityDetailModel : BaseModel
{
    public required DateTime StartTime { get; set; }
    public required DateTime FinishTime { get; set; }
    public required ActivityType ActivityType { get; set; }
    public string? CourseAbbreviation { get; set; }
    public Guid? CourseId { get; set; }
    public string? Description { get; set; }
    public int? Points { get; set; }
    
    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.Empty,
        StartTime = DateTime.Now,
        FinishTime = DateTime.Now,
        CourseAbbreviation = string.Empty,
        Description = string.Empty,
        Points = 0,
        ActivityType = ActivityType.Exercise,
        CourseId = Guid.Empty
    };
}