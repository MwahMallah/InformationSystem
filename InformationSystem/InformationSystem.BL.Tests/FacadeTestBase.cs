using InformationSystem.BL.Mappers;
using InformationSystem.Common.Tests;
using InformationSystem.Common.Tests.Factories;
using InformationSystem.DAL;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Xunit.Abstractions;

namespace InformationSystem.BL.Tests;

public class FacadeTestBase : IAsyncLifetime
{
    protected FacadeTestBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);
        InformationSystemDbContextSUT = DbContextFactory.CreateDbContext();

        ActivityModelMapper = new ActivityModelMapper();
        EvaluationModelMapper = new EvaluationModelMapper();
        var courseListModelMapper = new CourseListModelMapper();
        var studentListModelMapper = new StudentListModelMapper();
        
        CourseModelMapper = new CourseModelMapper(ActivityModelMapper, studentListModelMapper);
        StudentModelMapper = new StudentModelMapper
            (courseListModelMapper, ActivityModelMapper, EvaluationModelMapper);
        
        UnitOfWorkFactory = new UnitOfWorkFactory(DbContextFactory);
    }

    protected IDbContextFactory<InformationSystemDbContext> DbContextFactory { get; }
    protected InformationSystemDbContext InformationSystemDbContextSUT { get; set; }
    protected UnitOfWorkFactory UnitOfWorkFactory { get; }
    protected ActivityModelMapper ActivityModelMapper { get; }
    protected EvaluationModelMapper EvaluationModelMapper { get; } 
    protected CourseModelMapper CourseModelMapper { get; }
    protected StudentModelMapper StudentModelMapper { get; }
    
    public async Task InitializeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
        await dbx.Database.EnsureCreatedAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        await dbx.Database.EnsureDeletedAsync();
    }
}