using InformationSystem.Common.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;
using Xunit;
using InformationSystem.Common.Tests;
using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Tests
{
    public class DbContextCourseTests(ITestOutputHelper output) : DbContextTestsBase(output)
    {
        [Fact]
        public async Task AddNew_CourseWithoutStudents_Persisted()
        {
            var course = CourseSeeds.CourseEntity;

            InformationSystemDbContextSUT.Courses.Add(course);
            await InformationSystemDbContextSUT.SaveChangesAsync();

            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbCourse = await dbx.Courses
                .SingleAsync(i => i.Id == course.Id);
            DeepAssert.Equal(course, dbCourse);
        }
        
        [Fact]
        public async Task AddNew_CourseWithStudents_Persisted()
        {
            var course = CourseSeeds.EmptyCourseEntity with
            {
                Name = "zaklady programovani",
                Description = "c lang course",
                Abbreviation = "IZP",
                Students = new List<StudentEntity> {
                    StudentSeeds.EmptyStudentEntity with
                    {
                        FirstName = "Ilya",
                        LastName = "Volkov",
                        StartYear = 2020,
                        Group = "1A"
                    },
                    StudentSeeds.EmptyStudentEntity with
                    {
                        FirstName = "Tomas",
                        LastName = "Dvorak",
                        StartYear = 2022,
                        Group = "1B"
                    }
                }
            };
            
            
            InformationSystemDbContextSUT.Courses.Add(course);
            await InformationSystemDbContextSUT.SaveChangesAsync();

            //Assert
            await using var dbx = await DbContextFactory.CreateDbContextAsync();
            var dbCourse = await dbx.Courses
                .Include(i => i.Students)
                .SingleAsync(i => i.Id == course.Id);
            DeepAssert.Equal(course, dbCourse);
        }
        

        [Fact]
        public async Task GetAll_Courses_ContainsSeededCourse()
        {
            //Act
            var courses = await InformationSystemDbContextSUT.Courses.ToListAsync();

            //Assert
            DeepAssert.Contains(CourseSeeds.CourseEntity, courses);
        }

        [Fact]
        public async Task GetById_Course()
        {
            //Act
            var course = await InformationSystemDbContextSUT.Courses
                .SingleAsync(i => i.Id == CourseSeeds.CourseEntity.Id);
        
            //Assert
            DeepAssert.Equal(CourseSeeds.CourseEntity with { Students = Array.Empty<StudentEntity>() }, course);
        }
        //
        // [Fact]
        // public async Task GetById_IncludingIngredients_Recipe()
        // {
        //     //Act
        //     var entity = await CookBookDbContextSUT.Recipes
        //         .Include(i => i.Ingredients)
        //         .ThenInclude(i => i.Ingredient)
        //         .SingleAsync(i => i.Id == RecipeSeeds.RecipeEntity.Id);
        //
        //     //Assert
        //     DeepAssert.Equal(RecipeSeeds.RecipeEntity, entity);
        // }
        //
        // [Fact]
        // public async Task Update_Recipe_Persisted()
        // {
        //     //Arrange
        //     var baseEntity = RecipeSeeds.RecipeEntityUpdate;
        //     var entity =
        //         baseEntity with
        //         {
        //             Name = baseEntity.Name + "Updated",
        //             Description = baseEntity.Description + "Updated",
        //             Duration = default,
        //             FoodType = FoodType.None,
        //             ImageUrl = baseEntity.ImageUrl + "Updated",
        //         };
        //
        //     //Act
        //     CookBookDbContextSUT.Recipes.Update(entity);
        //     await CookBookDbContextSUT.SaveChangesAsync();
        //
        //     //Assert
        //     await using var dbx = await DbContextFactory.CreateDbContextAsync();
        //     var actualEntity = await dbx.Recipes.SingleAsync(i => i.Id == entity.Id);
        //     DeepAssert.Equal(entity, actualEntity);
        // }
        //
        // [Fact]
        // public async Task Delete_RecipeWithoutIngredients_Deleted()
        // {
        //     //Arrange
        //     var baseEntity = RecipeSeeds.RecipeEntityDelete;
        //
        //     //Act
        //     CookBookDbContextSUT.Recipes.Remove(baseEntity);
        //     await CookBookDbContextSUT.SaveChangesAsync();
        //
        //     //Assert
        //     Assert.False(await CookBookDbContextSUT.Recipes.AnyAsync(i => i.Id == baseEntity.Id));
        // }
        //
        // [Fact]
        // public async Task DeleteById_RecipeWithoutIngredients_Deleted()
        // {
        //     //Arrange
        //     var baseEntity = RecipeSeeds.RecipeEntityDelete;
        //
        //     //Act
        //     CookBookDbContextSUT.Remove(
        //         CookBookDbContextSUT.Recipes.Single(i => i.Id == baseEntity.Id));
        //     await CookBookDbContextSUT.SaveChangesAsync();
        //
        //     //Assert
        //     Assert.False(await CookBookDbContextSUT.Recipes.AnyAsync(i => i.Id == baseEntity.Id));
        // }
        //
        // [Fact]
        // public async Task Delete_RecipeWithIngredientAmounts_Deleted()
        // {
        //     //Arrange
        //     var baseEntity = RecipeSeeds.RecipeForIngredientAmountEntityDelete;
        //
        //     //Act
        //     CookBookDbContextSUT.Recipes.Remove(baseEntity);
        //     await CookBookDbContextSUT.SaveChangesAsync();
        //
        //     //Assert
        //     Assert.False(await CookBookDbContextSUT.Recipes.AnyAsync(i => i.Id == baseEntity.Id));
        //     Assert.False(await CookBookDbContextSUT.IngredientAmountEntities
        //         .AnyAsync(i => baseEntity.Ingredients.Select(ingredientAmount => ingredientAmount.Id).Contains(i.Id)));
        // }
    }
}
