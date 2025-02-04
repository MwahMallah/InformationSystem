﻿using InformationSystem.DAL.Entities;

namespace InformationSystem.DAL.Mappers;

public class CourseEntityMapper : IEntityMapper<CourseEntity>
{
    public void MapToExistingEntity(CourseEntity existingEntity, CourseEntity newEntity)
    {
        existingEntity.Name = newEntity.Name;
        existingEntity.Description = newEntity.Description;
        existingEntity.Abbreviation = newEntity.Abbreviation;
        existingEntity.Students = newEntity.Students;
        existingEntity.Activities = newEntity.Activities;
        existingEntity.MaxStudents = newEntity.MaxStudents;
        existingEntity.Credits = newEntity.Credits;
    }
}