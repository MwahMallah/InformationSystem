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
        Id = Guid.Parse("62C9F7A6-5c58-43B3-B8C0-B5FCFCC6DC2E"),
        StartTime = default,
        FinishTime = default,
        ActivityType = ActivityType.Exercise,
        RoomType = RoomType.MathClass,
        Description = "C# exercise"
    };

    public static readonly ActivityEntity ActivityExam = new()
    {
        Id = Guid.Parse("A7FFA756-5c58-43B3-B8C0-B5FCFCC6DC2E"),
        StartTime = default,
        FinishTime = default,
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