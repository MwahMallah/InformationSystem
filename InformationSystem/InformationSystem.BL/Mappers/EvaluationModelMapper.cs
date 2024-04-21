using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public class EvaluationModelMapper: 
    ModelMapperBase<EvaluationEntity, EvaluationDetailModel, EvaluationListModel>
{
    public override EvaluationListModel MapToListModel(EvaluationEntity entity)
        => new EvaluationListModel
        {
            Id = entity.Id,
            StudentId = entity.StudentId,
            ActivityId = entity.ActivityId,
            StudentFullName = entity.Student != null?  
                             $"{entity.Student.FirstName} {entity.Student.LastName}"
                             : string.Empty
        };

    public override EvaluationDetailModel MapToDetailModel(EvaluationEntity entity)
        => new EvaluationDetailModel
        {
            Id = entity.Id,
            StudentId = entity.StudentId,
            ActivityId = entity.ActivityId,
            Points = entity.Points,
            Description = entity.Description
        };


    public override EvaluationEntity MapToEntity(EvaluationDetailModel model)
        => new EvaluationEntity
        {
            Id = model.Id,
            Description = model.Description,
            StudentId = model.StudentId,
            ActivityId = model.ActivityId
        };
}