using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers.Interfaces;

public interface ICourseModelMapper : IModelMapper<CourseEntity, CourseDetailModel, CourseListModel>
{
    public CourseListModel MapToListModel(CourseDetailModel? model);
}