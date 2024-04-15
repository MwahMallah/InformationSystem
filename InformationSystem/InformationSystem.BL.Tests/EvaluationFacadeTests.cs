using InformationSystem.BL.Facades;
using InformationSystem.BL.Models;
using InformationSystem.Common.Tests;
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
    public async Task AddEvaluationWithoutCourses()
    {
        var evaluation = new EvaluationDetailModel
        {
            Id = Guid.NewGuid(),
            Description = "0/30 for this submission"
        };

        // Act: Attempt to save the new activity
        evaluation = await _evaluationFacadeSUT.SaveAsync(evaluation);
        //Assert
        await using var dbxAssert = await DbContextFactory.CreateDbContextAsync();
        
        var evaluationFromDb = await dbxAssert
            .Evaluations.SingleAsync(i => i.Id == evaluation.Id);
        var mapped = EvaluationModelMapper.MapToDetailModel(evaluationFromDb);
        DeepAssert.Equal(evaluation, mapped);
    }
}