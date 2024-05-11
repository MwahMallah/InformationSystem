using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformationSystem.DAL.Entities;

public record EvaluationEntity : IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Points { get; set; }
    [MaxLength(1000)]
    public string? Description { get; set; }

    [ForeignKey(nameof(StudentId))]
    public StudentEntity? Student { get; set; }
    public Guid? StudentId { get; set; }

    [ForeignKey((nameof(ActivityId)))]
    public ActivityEntity? Activity { get; set; }
    public Guid? ActivityId { get; set; }
}