using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Facades.Interfaces;

public interface ICourseFacade : IFacade<CourseEntity, CourseDetailModel, CourseListModel>
{
    public Task<IEnumerable<CourseListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true);
}