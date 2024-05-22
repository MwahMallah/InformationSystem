using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Facades.Interfaces;

public interface IEvaluationFacade : IFacade<EvaluationEntity, EvaluationDetailModel, EvaluationListModel>
{
    public Task<IEnumerable<EvaluationListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true);
    
    public Task<IEnumerable<EvaluationListModel>> GetStudentEvaluationsAsync(
        Guid studentId);

}