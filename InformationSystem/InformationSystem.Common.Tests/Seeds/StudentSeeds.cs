using InformationSystem.Common.Enums;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class StudentSeeds
{
    public static readonly StudentEntity EmptyStudentEntity = new()
    {
        Id = default, 
        FirstName = default!,
        LastName = default!,
        ImageUrl = default!,
        Group = default!,
        CurrentYear = default,
        StudentCourses = default!
    };
    
    public static readonly StudentEntity StudentEntity = new()
    {
        Id = Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        FirstName = "Maksim",
        LastName = "Dubrovin",
        ImageUrl = null,
        Group = "2B",
        CurrentYear = 2,
        // CourseId = ,
        // StudentCourses = 
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly StudentEntity StudentWithoutCourses = StudentEntity with
    {
        Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"), StudentCourses = Array.Empty<StudentCourseEntity>()
    };


    static StudentSeeds()
    {
        // StudentEntity.StudentCourses.Add(IngredientAmountSeeds.IngredientAmountEntity1);
        // StudentEntity.StudentCourses.Add(IngredientAmountSeeds.IngredientAmountEntity2);
        //
        // RecipeForIngredientAmountEntityDelete.Ingredients.Add(IngredientAmountSeeds.IngredientAmountEntityDelete);
    }

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentEntity>().HasData(
            StudentEntity with { StudentCourses = Array.Empty<StudentCourseEntity>() },
            StudentWithoutCourses,
            EmptyStudentEntity
        );
    }
    
}