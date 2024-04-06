namespace InformationSystem.BL.Models;

public record EvaluationDetailModel : BaseModel
{
    public int Points { get; set; }
    public Guid StudentId { get; set; }
    public Guid ActivityId { get; set; }
    public required string Description { get; set; }
    
    public static EvaluationDetailModel Empty => new()
    {
        Id = Guid.Empty,
        StudentId = Guid.Empty,
        ActivityId = Guid.Empty,
        Description = string.Empty
    };
}