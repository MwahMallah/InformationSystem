namespace InformationSystem.BL.Models;

public record CourseListModel: BaseModel
{
    public required string Abbreviation { get; set; }
    public required string Name { get; set; }
    public int MaxStudents { get; set; }
    public int Credits { get; set; }

    public static CourseListModel Empty => new()
    {
        Id = Guid.Empty,
        Abbreviation = string.Empty,
        Name = string.Empty
    };

}