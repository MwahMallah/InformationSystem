using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Facades.Interfaces;

public interface IActivityFacade : IFacade<ActivityEntity, ActivityDetailModel, ActivityListModel>
{
    public Task<IEnumerable<ActivityListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true);
}