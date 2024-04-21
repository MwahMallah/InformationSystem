using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class EvaluationSeeds
{
    public static readonly EvaluationEntity EmptyEvaluationEntity = new()
    {
        Id = Guid.Empty, 
        Description = default!,
    };

    public static readonly EvaluationEntity BadEvaluation = EmptyEvaluationEntity with
    {
        Id = Guid.Parse("A2FBCF52-5c58-43B3-B8C0-B5FCFFF6DC2E"),
        Description = "Very bad student",
        Points = 0
    };
    
    public static readonly EvaluationEntity GoodEvaluation = EmptyEvaluationEntity with
    {
        Id = Guid.Parse("1BE3451F-5c58-43B3-B8C0-B5FCFFF6DC2E"),
        Description = "Very good student",
        Points = 5
    };
    
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EvaluationEntity>().HasData(
            BadEvaluation,
            GoodEvaluation
        );
    }
}