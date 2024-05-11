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
            StudentFullName = entity.Student != null?  
                $"{entity.Student.FirstName} {entity.Student.LastName}"
                : string.Empty,
            ActivityId = entity.ActivityId,
            CourseAbbreviation = entity.Activity?.Course != null?
                                $"{entity.Activity.Course.Abbreviation}":
                                string.Empty,
            Points = entity.Points
        };

    public override EvaluationDetailModel MapToDetailModel(EvaluationEntity entity)
        => new EvaluationDetailModel
        {
            Id = entity.Id,
            StudentId = entity.StudentId,
            StudentFullName = entity.Student != null?  
                $"{entity.Student.FirstName} {entity.Student.LastName}"
                : string.Empty,
            CourseAbbreviation = entity.Activity?.Course != null?
                entity.Activity.Course.Abbreviation:
                string.Empty,
            ActivityId = entity.ActivityId,
            Points = entity.Points,
            Description = entity.Description
        };


    public override EvaluationEntity MapToEntity(EvaluationDetailModel model)
        => new EvaluationEntity
        {
            Id = model.Id,
            Points = model.Points,
            Description = model.Description,
            StudentId = model.StudentId,
            ActivityId = model.ActivityId
        };
}