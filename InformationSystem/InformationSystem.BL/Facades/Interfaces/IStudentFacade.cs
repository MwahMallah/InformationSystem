using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Facades.Interfaces;

public interface IStudentFacade : IFacade<StudentEntity, StudentDetailModel, StudentListModel>
{
    public Task<IEnumerable<StudentListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true);
}