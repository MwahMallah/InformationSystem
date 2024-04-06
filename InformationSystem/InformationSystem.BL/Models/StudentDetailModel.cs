using System.Collections.ObjectModel;

namespace InformationSystem.BL.Models;

public record StudentDetailModel : BaseModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Group { get; set; }
    public required int CurrentYear { get; set; }
    public Uri? ImageUrl { get; set; }

    public ObservableCollection<CourseListModel> Courses { get; set; } = [];
    public ObservableCollection<ActivityListModel> Activities { get; set; } = [];
    public ObservableCollection<EvaluationListModel> Evaluations { get; set; } = [];

    public static StudentDetailModel Empty => new()
    {
        Id = Guid.Empty,
        FirstName = string.Empty,
        LastName = string.Empty,
        Group = string.Empty,
        CurrentYear = 0
    };
}