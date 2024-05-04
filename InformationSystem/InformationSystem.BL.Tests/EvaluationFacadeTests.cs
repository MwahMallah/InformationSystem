using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Tests;
using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace InformationSystem.BL.Tests;

public class EvaluationFacadeTests: FacadeTestBase
{
    private readonly EvaluationFacade _evaluationFacadeSUT;
    
    public EvaluationFacadeTests(ITestOutputHelper output) : base(output)
    {
        _evaluationFacadeSUT = new EvaluationFacade(UnitOfWorkFactory, EvaluationModelMapper);
    }
    
    [Fact]
    public async Task AddEvaluationWithStudentAndActivity()
    {
        var evaluation = new EvaluationDetailModel
        {
            Description = "0/30 for this submission",
            StudentId = StudentSeeds.StudentIlya.Id,
            ActivityId = ActivitySeeds.ActivityExercise.Id
        };

        // Act: Attempt to save the new activity
        evaluation = await _evaluationFacadeSUT.SaveAsync(evaluation);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var evaluationFromDb = await dbxAssert
            .Evaluations.SingleAsync(e => e.Id == evaluation.Id);
        var mapped = EvaluationModelMapper.MapToDetailModel(evaluationFromDb);
        DeepAssert.Equal(evaluation, mapped);
    }
    
    [Fact]
    public async Task GetEvaluationById()
    {
        var evaluation = await _evaluationFacadeSUT.GetAsync(EvaluationSeeds.GoodEvaluation.Id);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var evaluationFromDb = await dbxAssert
            .Evaluations.SingleAsync(e => e.Id == evaluation.Id);
        var mapped = EvaluationModelMapper.MapToDetailModel(evaluationFromDb);
        DeepAssert.Equal(evaluation, mapped);
    }
    
    [Fact]
    public async Task GetAllEvaluations()
    {
        var evaluations = await _evaluationFacadeSUT.GetAsync();
        var evaluation = evaluations.SingleOrDefault(e => e.Id == EvaluationSeeds.BadEvaluation.Id);
        DeepAssert.Equal(evaluation, 
            EvaluationModelMapper.MapToListModel(EvaluationSeeds.BadEvaluation));
    }
    
    [Fact]
    public async Task Update_EvaluationUpdated()
    {
        var evaluation = new EvaluationDetailModel
        {
            Description = "0/30 for this submission",
            StudentId = StudentSeeds.StudentIlya.Id,
            ActivityId = ActivitySeeds.ActivityExercise.Id
        };
        
        evaluation = await _evaluationFacadeSUT.SaveAsync(evaluation);
        evaluation.Description = "30/30 for this submission";
        
        evaluation = await _evaluationFacadeSUT.SaveAsync(evaluation);
        
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var evaluationFromDb = await dbxAssert.Evaluations
            .Include(e=>e.Student)
            .Include(e => e.Activity)
            .SingleAsync(e => e.Id == evaluation.Id);
        
        DeepAssert.Equal(evaluation, EvaluationModelMapper.MapToDetailModel(evaluationFromDb));
    }
    
    [Fact]
    public async Task DeleteById_EvaluationDeleted()
    {
        await _evaluationFacadeSUT.DeleteAsync(EvaluationSeeds.BadEvaluation.Id);
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        var found = await dbxAssert.Evaluations.AnyAsync(e => e.Id == EvaluationSeeds.BadEvaluation.Id);
        Assert.False(found);
    }
    
    [Fact]
    public async Task DeleteById_NonExistentEvaluationThrows()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(async()=>
            await _evaluationFacadeSUT.DeleteAsync(Guid.Empty)
        );
    }
    
}