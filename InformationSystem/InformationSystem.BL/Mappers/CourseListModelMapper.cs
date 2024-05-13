using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class CourseListModelMapper
    : ListModelMapperBase<CourseEntity, CourseListModel>, ICourseListModelMapper
{
    public override CourseListModel MapToListModel(CourseEntity? entity)
        =>  entity is null?
            CourseListModel.Empty 
            : new CourseListModel 
            {
                Id = entity.Id,
                Abbreviation = entity.Abbreviation,
                Name = entity.Name,
                MaxStudents = entity.MaxStudents,
                Credits = entity.Credits
            };
}