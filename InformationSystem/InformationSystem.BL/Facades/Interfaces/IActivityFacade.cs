using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Facades.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityDetailModel, ActivityListModel>
{
    public Task<IEnumerable<ActivityListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true);

    public Task<IEnumerable<ActivityListModel>> GetFromCourseAsync(
        Guid courseId, DateTime? startTime = null, DateTime? finishTime = null);

    public Task<IEnumerable<ActivityListModel>> FilterByTime(
        IEnumerable<ActivityListModel> activities, DateTime? startTime = null, DateTime? finishTime = null);
}