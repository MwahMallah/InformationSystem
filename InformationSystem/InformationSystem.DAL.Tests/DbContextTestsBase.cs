﻿using InformationSystem.Common.Tests.Factories;
using InformationSystem.Common.Tests;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;

namespace InformationSystem.DAL.Tests;

public class DbContextTestsBase : IAsyncLifetime
{
    protected DbContextTestsBase(ITestOutputHelper output)
    {
        XUnitTestOutputConverter converter = new(output);
        Console.SetOut(converter);

        DbContextFactory = new DbContextSqLiteTestingFactory(GetType().FullName!, seedTestingData: true);
        InformationSystemDbContextSUT = DbContextFactory.CreateDbContext();
    }

    protected IDbContextFactory<InformationSystemDbContext> DbContextFactory { get; }
    protected InformationSystemDbContext InformationSystemDbContextSUT { get; }


    public async Task InitializeAsync()
    {
        await InformationSystemDbContextSUT.Database.EnsureDeletedAsync();
        await InformationSystemDbContextSUT.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await InformationSystemDbContextSUT.Database.EnsureDeletedAsync();
        await InformationSystemDbContextSUT.DisposeAsync();
    }
}