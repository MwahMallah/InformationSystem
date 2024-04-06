using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;

namespace InformationSystem.BL.Facades;

public class StudentFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<StudentEntity, StudentDetailModel, StudentListModel> modelMapper) 
    : FacadeBase<StudentEntity, StudentDetailModel, StudentListModel, StudentEntityMapper>
        (unitOfWorkFactory, modelMapper)
{
    
}