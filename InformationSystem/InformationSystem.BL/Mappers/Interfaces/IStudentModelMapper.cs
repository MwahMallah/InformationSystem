using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers.Interfaces;

public interface IStudentModelMapper : IModelMapper<StudentEntity, StudentDetailModel, StudentListModel>
{
    
}