using InformationSystem.Common.Enums;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class StudentSeeds
{
    public static readonly StudentEntity EmptyStudentEntity = new()
    {
        Id = Guid.Empty, 
        FirstName = default!,
        LastName = default!,
        ImageUrl = default!,
        Group = default!,
        StartYear = default,
        Courses = default!
    };
    
    public static readonly StudentEntity StudentEntity = new()
    {
        Id = Guid.Parse(input: "fabde0cd-eefe-443f-baf6-3d96cc2cbf2e"),
        FirstName = "Maksim",
        LastName = "Dubrovin",
        ImageUrl = null,
        Group = "2B",
        StartYear = 2022,
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly StudentEntity StudentMaksim = StudentEntity with
    {
        Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"), 
        Courses = Array.Empty<CourseEntity>()
    };
    
    public static readonly StudentEntity StudentIlya = StudentMaksim with
    {
        Id = Guid.Parse("17D8F7B6-0E51-4183-B8C0-B5FCFFF6DC2E"), 
        Courses = Array.Empty<CourseEntity>(),
        FirstName = "Ilya",
        LastName = "Volkov"
    };
    
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentEntity>().HasData(
            StudentMaksim,
            StudentIlya
        );
    }
    
}