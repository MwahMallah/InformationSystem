using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InformationSystem.DAL.Entities;

public class EvaluationEntity : IEntity 
{
    public Guid Id { get; set; }
    public int Points { get; set; }
    [MaxLength(1000)]
    public required string Description { get; set; }

    [ForeignKey(nameof(StudentId))]
    public required StudentEntity Student { get; set; }
    public Guid StudentId { get; set; }

    public required ActivityEntity Activity { get; set; }
    public Guid ActivityId { get; set; }
}