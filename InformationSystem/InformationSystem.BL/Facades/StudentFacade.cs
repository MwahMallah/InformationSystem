using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class StudentFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<StudentEntity, StudentDetailModel, StudentListModel> modelMapper) 
    : FacadeBase<StudentEntity, StudentDetailModel, StudentListModel, StudentEntityMapper>
        (unitOfWorkFactory, modelMapper)
{
    public override async Task<StudentDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
        
        var query = repository.Get()
            .Include(s=>s.Courses);
        
        var entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }
    
    public override async Task<StudentDetailModel> SaveAsync(StudentDetailModel model)
    {
        StudentDetailModel result;
        StudentEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
        
        var courseRepository = uow.GetRepository<CourseEntity, CourseEntityMapper>();

        foreach (var courseId in model.Courses.Select(c=>c.Id))
        {
            var course = await courseRepository.Get().SingleOrDefaultAsync(c=>c.Id==courseId);
            
            if (course != null)
            {
                entity.Courses.Add(course);
            }
            else
            {
                throw new InvalidOperationException($"Course with ID {courseId} does not exist.");
            }
        }
        
        Func<IQueryable<StudentEntity>, IQueryable<StudentEntity>> include 
            = query => query.Include(s => s.Courses);
        
        if (await repository.ExistsAsync(entity))
        {
            StudentEntity updatedEntity = await repository.UpdateAsync(entity, include);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            StudentEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }
}