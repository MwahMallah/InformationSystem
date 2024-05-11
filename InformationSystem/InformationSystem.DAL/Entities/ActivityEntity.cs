using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InformationSystem.Common.Enums;

namespace InformationSystem.DAL.Entities;

public record ActivityEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime StartTime{ get; set; }
    public DateTime FinishTime { get; set; }
    public ActivityType ActivityType { get; set; }
    public int? MaxPoints { get; set; }
    public RoomType RoomType { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }
    [ForeignKey(nameof(CourseId))]
    public CourseEntity? Course { get; set; }
    public Guid? CourseId { get; set; }
    public ICollection<EvaluationEntity> Evaluations { get; set; } = new List<EvaluationEntity>();
}