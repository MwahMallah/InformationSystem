namespace InformationSystem.DAL.Entities;

public class EvaluationEntity
{
    public int Points { get; set; }
    public required string Description { get; set; }

    public required StudentEntity Student { get; set; }
    public Guid StudentId { get; set; }

    public required ActivityEntity Activity { get; set; }
    public Guid ActivityId { get; set; }
}