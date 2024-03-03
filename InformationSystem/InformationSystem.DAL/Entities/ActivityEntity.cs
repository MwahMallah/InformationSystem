using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformationSystem.DAL.Entities;

public class ActivityEntity
{
    public Guid Id { get; set; }
    public DateTime StartTime{ get; set; }
    public DateTime FinishTime { get; set; }
    public ActivityType ActivityType { get; set; }
    public RoomType RoomType { get; set; }
    [MaxLength(1000)]
    public required string Description { get; set; }

    public required CourseEntity Course { get; init; }
    public Guid CourseId { get; set; }

    [ForeignKey(nameof(EvaluationId))]
    public required EvaluationEntity Evaluation { get; init; }
    public Guid EvaluationId { get; set; }
}