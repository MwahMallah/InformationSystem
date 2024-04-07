using InformationSystem.Common.Enums;

namespace InformationSystem.BL.Models;

public record ActivityDetailModel : BaseModel
{
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public ActivityType ActivityType { get; set; }
    public required string CourseAbbreviation { get; set; }
    public string? Description { get; set; }
    public int? Points { get; set; }
    
    public static ActivityDetailModel Empty => new()
    {
        Id = Guid.Empty,
        StartTime = DateTime.Now,
        FinishTime = DateTime.Now,
        CourseAbbreviation = string.Empty,
        Description = string.Empty,
        Points = 0
    };
}