using InformationSystem.Common.Enums;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.Common.Tests.Seeds;

public static class ActivitySeeds
{
    public static readonly ActivityEntity ActivityEntity = new()
    {
        Id = Guid.NewGuid(),
        ActivityType = ActivityType.Exam,
        RoomType = RoomType.ComputerClass,
        Course = CourseSeeds.CourseEntity,
        CourseId = CourseSeeds.CourseEntity.Id
    };
    public static void Seed(this ModelBuilder modelBuilder) =>
        modelBuilder.Entity<ActivityEntity>().HasData(
            ActivityEntity
        );
    
}