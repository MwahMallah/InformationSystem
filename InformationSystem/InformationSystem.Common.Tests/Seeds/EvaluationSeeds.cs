using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class EvaluationSeeds
{
    public static readonly EvaluationEntity EmptyEvaluationEntity = new()
    {
        Id = default,
        Description = "",
        Points = default,
        Student = new() { Id = default, FirstName = "", LastName = "", Group = "" },
        Activity = new(){Id = default, Course = new(){Id = default, Name = "", Abbreviation = ""}}
    };
    public static readonly EvaluationEntity EvaluationEntity = new()
    {
        Id = Guid.NewGuid(),
        Description = "exam eval.",
        Points = 10,
        Student = new() { Id = default, FirstName = "Alex", LastName = "Mercy", Group = "1C" },
        Activity = new(){Id = Guid.NewGuid(), Course = new(){Id = Guid.NewGuid(), Name = "programming", Abbreviation = "IZP"}}
    };

    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<EvaluationEntity>().HasData(EvaluationEntity);
}