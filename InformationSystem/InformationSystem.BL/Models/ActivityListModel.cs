using System.Collections.ObjectModel;
using InformationSystem.Common.Enums;

namespace InformationSystem.BL.Models;

public record ActivityListModel : BaseModel
{
    public DateTime StartTime { get; set; }
    public DateTime FinishTime { get; set; }
    public ActivityType ActivityType { get; set; }
    public int MaxPoints { get; set; }
    public required string CourseAbbreviation { get; set; }
    public int? Points { get; set; }
    
    public static ActivityListModel Empty => new()
    {
        Id = Guid.Empty,
        StartTime = DateTime.Now,
        FinishTime = DateTime.Now,
        CourseAbbreviation = string.Empty,
        Points = 0
    };
}