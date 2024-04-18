namespace InformationSystem.BL.Models;

public record EvaluationListModel : BaseModel
{
    public int Points { get; set; }
    public string? StudentLastName { get; set; }
    public required Guid StudentId { get; set; }
    public string? CourseAbbreviation { get; set; }
    public required Guid ActivityId { get; set; }

    public static EvaluationListModel Empty => new()
    {
        Id = Guid.Empty,
        StudentId = Guid.Empty,
        ActivityId = Guid.Empty
    };
}