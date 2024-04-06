using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;

namespace InformationSystem.BL.Models;

public record CourseDetailModel : BaseModel
{
    public required string Abbreviation { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public int MaxStudents { get; set; }
    public int Credits { get; set; }
    public ObservableCollection<ActivityListModel> Activities { get; set; } = [];
    public ObservableCollection<StudentListModel> Students { get; set; } = [];

    public static CourseDetailModel Empty => new()
    {
        Id = Guid.Empty, 
        Abbreviation = string.Empty,
        Name = string.Empty,
        Description = string.Empty,
        Credits = 0,
        MaxStudents = 0
    };

}