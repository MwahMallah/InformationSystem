using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;

namespace InformationSystem.BL.Facades;

public class CourseFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<CourseEntity, CourseDetailModel, CourseListModel> modelMapper) 
    : FacadeBase<CourseEntity, CourseDetailModel, CourseListModel, CourseEntityMapper>(unitOfWorkFactory, modelMapper)
{
    
}