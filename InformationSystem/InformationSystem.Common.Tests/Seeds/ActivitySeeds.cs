using InformationSystem.Common.Enums;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class ActivitySeeds
{
    public static readonly ActivityEntity EmptyActivityEntity = new()
    {
        Id = default,
        ActivityType = default,
        RoomType = default,
        Course = new CourseEntity() {Id = default, Name = "", Abbreviation = ""},
    };
    public static readonly ActivityEntity ActivityEntity = new()
    {
        Id = Guid.NewGuid(),
        ActivityType = ActivityType.Exam,
        RoomType = RoomType.ComputerClass,
        Course = new CourseEntity() {Id = Guid.NewGuid(), Name = "Databases", Abbreviation = "IDS"},
    };
    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ActivityEntity>().HasData(
            ActivityEntity,
            EmptyActivityEntity
        );
    
}