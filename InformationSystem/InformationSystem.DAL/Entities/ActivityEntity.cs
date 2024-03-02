namespace InformationSystem.DAL.Entities;

public class ActivityEntity
{
    public Guid Id { get; set; }
    public DateTime StartTime{ get; set; }
    public DateTime FinishTime { get; set; }
    public ActivityType ActivityType { get; set; }
    public RoomType RoomType { get; set; }
    public required string Description { get; set; }

    public required CourseEntity Course { get; init; }
    public Guid CourseId { get; set; }

    public required EvaluationEntity Evaluation { get; init; }
    public Guid EvaluationId { get; set; }
}