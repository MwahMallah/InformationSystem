using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;
using InformationSystem.Common.Tests;
using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Tests;

public class DbContextEvaluationTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_Evaluation_Persisted()
    {
        var evaluation = EvaluationSeeds.EvaluationEntity;

        InformationSystemDbContextSUT.Evaluations.Add(evaluation);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var dbEvaluation = await dbx.Evaluations
            .SingleAsync(i => i.Id == evaluation.Id);
        DeepAssert.Equal(evaluation, dbEvaluation);
    }

    [Fact]
    public async Task GetById_Evaluation()
    {
        var evaluation = await InformationSystemDbContextSUT.Evaluations
            .SingleAsync(i => i.Id == EvaluationSeeds.EvaluationEntity.Id);

        //Assert
        DeepAssert.Equal(EvaluationSeeds.EvaluationEntity, evaluation);
    }

    [Fact]
    public async Task Update_Evaluation_Persisted()
    {
        //Arrange
        var baseEntity = EvaluationSeeds.EvaluationEntity;
        var entity =
            baseEntity with
            {
                Description = baseEntity.Description + "Updated",
                Points = baseEntity.Points + 5
            };

        InformationSystemDbContextSUT.Evaluations.Update(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Evaluations.SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }

    [Fact]
    public async Task Delete_Evaluation_Deleted()
    {
        var baseEntity = EvaluationSeeds.EvaluationEntity;

        InformationSystemDbContextSUT.Evaluations.Remove(baseEntity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        //Assert
        Assert.False(await InformationSystemDbContextSUT.Evaluations.AnyAsync(i => i.Id == baseEntity.Id));
    }
}
