using InformationSystem.Common.Tests;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;

namespace InformationSystem.DAL.Tests;

public class DbContextStudentTests(ITestOutputHelper output) : DbContextTestsBase(output)
{
    [Fact]
    public async Task AddNew_RecipeWithoutIngredients_Persisted()
    {
        //Arrange
        StudentEntity entity = new()
        {
            Id = Guid.Parse("C5DE45D7-64A0-4E8D-AC7F-BF5CFDFB0EFC"),
            FirstName = "Maksim",
            LastName = "Dubrovin",
            ImageUrl =
                "https://upload.wikimedia.org/wikipedia/commons/thumb/7/78/Salt_shaker_on_white_background.jpg/800px-Salt_shaker_on_white_background.jpg",
            Group = "2B"
        };

        //Act
        InformationSystemDbContextSUT.Students.Add(entity);
        await InformationSystemDbContextSUT.SaveChangesAsync();

        //Assert
        await using var dbx = await DbContextFactory.CreateDbContextAsync();
        var actualEntity = await dbx.Students
            .SingleAsync(i => i.Id == entity.Id);
        DeepAssert.Equal(entity, actualEntity);
    }
}