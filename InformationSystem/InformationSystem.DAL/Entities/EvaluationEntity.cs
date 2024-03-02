using System.ComponentModel.DataAnnotations.Schema;

namespace InformationSystem.DAL.Entities;

public class EvaluationEntity {
    public Guid Id { get; set; }
    public int Points { get; set; }
    public required string Description { get; set; }

    [ForeignKey(nameof(StudentId))]
    public StudentEntity? Student { get; set; }
    public Guid StudentId { get; set; }

    public required ActivityEntity Activity { get; set; }
    public Guid ActivityId { get; set; }
}