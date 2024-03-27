﻿using System.ComponentModel.DataAnnotations;

namespace InformationSystem.DAL.Entities;

public record StudentEntity 
{
    public Guid Id { get; set; } = Guid.NewGuid(); 
    [MaxLength(50)]
    public required string FirstName { get; set; }
    [MaxLength(50)]
    public required string LastName { get; set; }
    public string? ImageUrl { get; set; }
    [MaxLength(5)]
    public required string Group { get; set; }
    public int StartYear { get; set; } 

    public ICollection<CourseEntity> Courses { get; set; } = new List<CourseEntity>();
}