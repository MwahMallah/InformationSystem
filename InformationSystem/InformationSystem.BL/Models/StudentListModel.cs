namespace InformationSystem.BL.Models;

public record StudentListModel : BaseModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Group { get; set; }
    public required int CurrentYear { get; set; }

    public static StudentListModel Empty => new()
    {
        Id = Guid.Empty,
        FirstName = string.Empty,
        LastName = string.Empty,
        Group = string.Empty,
        CurrentYear = 0
    };
}