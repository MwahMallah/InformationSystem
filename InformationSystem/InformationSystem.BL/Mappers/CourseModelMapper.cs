using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InformationSystem.BL.Mappers;

public class CourseModelMapper(IActivityModelMapper activityModelMapper,
                                IStudentListModelMapper studentListModelMapper): 
    ModelMapperBase<CourseEntity, CourseDetailModel, CourseListModel>, ICourseModelMapper
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

    public CourseListModel MapToListModel(CourseDetailModel? model)
        => model is null?
            CourseListModel.Empty 
            : new CourseListModel
            {
                Id = model.Id,
                Abbreviation = model.Abbreviation,
                Name = model.Name,
                MaxStudents = model.MaxStudents,
                Credits = model.Credits
            };
        

    public override CourseDetailModel MapToDetailModel(CourseEntity? entity)
        => entity is null?
            CourseDetailModel.Empty 
            : new CourseDetailModel
        {
            Id = entity.Id,
            Abbreviation = entity.Abbreviation,
            Name = entity.Name,
            MaxStudents = entity.MaxStudents,
            Credits = entity.Credits,
            Description = entity.Description,
            Activities = activityModelMapper.MapToListModel(entity.Activities).ToObservableCollection(),
            Students = studentListModelMapper.MapToListModel(entity.Students).ToObservableCollection()
        };

    public override CourseEntity MapToEntity(CourseDetailModel model)
        => new CourseEntity
        {
            Id = model.Id,
            Name = model.Name,
            Description = model.Description,
            Abbreviation = model.Abbreviation,
            MaxStudents = model.MaxStudents,
            Credits = model.Credits
        };
}