using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace InformationSystem.BL.Mappers;

public class CourseModelMapper(ActivityModelMapper activityModelMapper,
                                StudentListModelMapper studentListModelMapper): 
    ModelMapperBase<CourseEntity, CourseDetailModel, CourseListModel>
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
            Abbreviation = model.Abbreviation
        };
}