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
        ImageUrl = null,
        Group = "2B",
        StartYear = 2022,
        FirstName = string.Empty,
        LastName = string.Empty,
    };

    //To ensure that no tests reuse these clones for non-idempotent operations
    public static readonly StudentEntity StudentMaksim = StudentEntity with
    {
        FirstName = "Maksim",
        LastName = "Dubrovin",
        StartYear = 2022,
        Group = "7C",
        Id = Guid.Parse("98B7F7B6-0F51-43B3-B8C0-B5FCFFF6DC2E"), 
        Courses = Array.Empty<CourseEntity>()
    };
    
    public static readonly StudentEntity StudentMaksimSecond = StudentEntity with
    {
        FirstName = "Maksim",
        LastName = "Antonov",
        StartYear = 2021,
        Group = "7D",
        Id = Guid.Parse("A7FE02C1-0F51-43B3-B8C0-B5FCFFF6DC2E"), 
        Courses = Array.Empty<CourseEntity>()
    };
    
    public static readonly StudentEntity StudentIlya = StudentEntity with
    {
        FirstName = "Ilya",
        LastName = "Volkov",
        StartYear = 2023,
        Group = "9C",
        Id = Guid.Parse("17D8F7B6-0E51-4183-B8C0-B5FCFFF6DC2E"), 
        Courses = Array.Empty<CourseEntity>()
    };
    
    public static readonly StudentEntity StudentNastya = StudentEntity with
    {
        FirstName = "Nastya",
        LastName = "Mironova",
        StartYear = 2020,
        Group = "9C",
        Id = Guid.Parse("34B09E2B-0E51-4183-B8C0-B5FCFFF6DC2E"), 
        Courses = Array.Empty<CourseEntity>()
    };
    
    public static readonly StudentEntity StudentArtyom = StudentEntity with
    {
        FirstName = "Artyom",
        LastName = "Belitzkiy",
        StartYear = 2019,
        Group = "2A",
        Id = Guid.Parse("F2AFBCAA-0E51-4183-B8C0-B5FCFFF6DC2E"), 
        Courses = Array.Empty<CourseEntity>()
    };
    
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentEntity>().HasData(
            StudentMaksim,
            StudentIlya,
            StudentMaksimSecond,
            StudentNastya,
            StudentArtyom
        );
    }
    
}