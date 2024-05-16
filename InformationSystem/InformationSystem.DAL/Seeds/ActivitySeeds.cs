using System.Globalization;
using InformationSystem.Common.Enums;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.DAL.Seeds;

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
    
    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityEntity>()
            .HasData(
                ActivityExercise,
                ActivityExam
            );
    }
}