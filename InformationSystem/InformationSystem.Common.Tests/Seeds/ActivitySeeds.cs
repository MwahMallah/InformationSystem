using System.Globalization;
using InformationSystem.Common.Enums;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class ActivitySeeds
{
    public static readonly ActivityEntity EmptyActivityEntity = new()
    {
        Id = Guid.Empty, 
        Description = default!
    };

    public static readonly ActivityEntity ActivityExercise = new()
    {
        Id = Guid.Parse("62C9F7A6-5c58-43F1-C9F0-B6FCACB7EC2E"),
        StartTime = DateTime.ParseExact("07-05-2023:12-15", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        FinishTime = DateTime.ParseExact("07-05-2023:13-15", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        MaxPoints = 5,
        ActivityType = ActivityType.Exercise,
        RoomType = RoomType.MathClass,
        Description = "C# exercise"
    };
    
    public static readonly ActivityEntity ActivityExerciseIds = new()
    {
        Id = Guid.Parse("3FEEEDD6-5c58-43B3-BEC0-D51C1CC68C3E"),
        StartTime = DateTime.ParseExact("02-04-2023:14-30", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        FinishTime = DateTime.ParseExact("02-04-2023:16-00", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        MaxPoints = 10,
        ActivityType = ActivityType.Exercise,
        RoomType = RoomType.MathClass,
        Description = "IDS exercise"
    };

    public static readonly ActivityEntity ActivityExam = new()
    {
        Id = Guid.Parse("A7FFA756-5c58-43B3-BACD-BEFAFAC6DC2E"),
        StartTime = DateTime.ParseExact("01-05-2023:12-45", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        FinishTime = DateTime.ParseExact("01-05-2023:14-30", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        MaxPoints = 50,
        ActivityType = ActivityType.Exam,
        RoomType = RoomType.MathClass,
        Description = "IPK exam"
    };
    
    public static readonly ActivityEntity ActivityExamIds = new()
    {
        Id = Guid.Parse("B2ECD895-5D58-52BF-B8C0-B2FC4CC7EC2E"),
        StartTime = DateTime.ParseExact("21-03-2023:11-15", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        FinishTime = DateTime.ParseExact("21-03-2023:12-00", "dd-MM-yyyy:HH-mm", CultureInfo.InvariantCulture),
        MaxPoints = 40,
        ActivityType = ActivityType.Exam,
        RoomType = RoomType.MathClass,
        Description = "IDS exam"
    };

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>()
            .HasData(
                ActivityExercise,
                ActivityExam,
                ActivityExerciseIds,
                ActivityExamIds
            );
    }
}