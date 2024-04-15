using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;

namespace InformationSystem.BL.Facades;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<ActivityEntity, ActivityDetailModel, ActivityListModel> modelMapper) 
    : FacadeBase<ActivityEntity, ActivityDetailModel, 
        ActivityListModel, ActivityEntityMapper>(unitOfWorkFactory, modelMapper)
{
    
}