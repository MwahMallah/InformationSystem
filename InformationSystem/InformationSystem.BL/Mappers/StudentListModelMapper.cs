using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class StudentListModelMapper
    : ListModelMapperBase<StudentEntity, StudentListModel>, IStudentListModelMapper
{
    public override StudentListModel MapToListModel(StudentEntity? entity)
        => entity is null?
            StudentListModel.Empty : 
            new StudentListModel
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Group = entity.Group,
            CurrentYear = DateTime.Now.Year - entity.StartYear
        };
}