using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class EvaluationModelMapper: 
    ModelMapperBase<EvaluationEntity, EvaluationDetailModel, EvaluationListModel>
{
    public override EvaluationListModel MapToListModel(EvaluationEntity entity)
        => new EvaluationListModel
        {
            StudentId = entity.StudentId,
            ActivityId = entity.ActivityId,
            StudentLastName = entity.Student?.LastName ?? string.Empty,
        };

    public override EvaluationDetailModel MapToDetailModel(EvaluationEntity entity)
        => new EvaluationDetailModel
        {
            StudentId = entity.StudentId,
            ActivityId = entity.ActivityId,
            Points = entity.Points,
            Description = entity.Description
        };
        

    public override EvaluationEntity MapToEntity(EvaluationDetailModel model)
    {
        throw new NotImplementedException();
    }
}