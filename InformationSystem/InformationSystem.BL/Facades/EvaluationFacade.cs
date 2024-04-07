using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;

namespace InformationSystem.BL.Facades;

public class EvaluationFacade(IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<EvaluationEntity, EvaluationDetailModel, EvaluationListModel> modelMapper) 
    : FacadeBase<EvaluationEntity, EvaluationDetailModel, EvaluationListModel, EvaluationEntityMapper>
        (unitOfWorkFactory, modelMapper)
{
    
}