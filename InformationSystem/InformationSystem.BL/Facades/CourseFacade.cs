using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class CourseFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<CourseEntity, CourseDetailModel, CourseListModel> modelMapper) 
    : FacadeBase<CourseEntity, CourseDetailModel, CourseListModel, CourseEntityMapper>(unitOfWorkFactory, modelMapper)
{
    public override async Task<CourseDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        var query = repository.Get()
            .Include(c=>c.Students);
        
        var entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }
    
    public override async Task<CourseDetailModel> SaveAsync(CourseDetailModel model)
    {
        CourseDetailModel result;
        CourseEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

        foreach (var studentId in model.Students.Select(c=>c.Id))
        {
            var course = await studentRepository.Get().SingleOrDefaultAsync(c=>c.Id==studentId);
            
            if (course != null)
            {
                entity.Students.Add(course);
            }
            else
            {
                throw new InvalidOperationException($"Student with ID {studentId} does not exist.");
            }
        }
        
        Func<IQueryable<CourseEntity>, IQueryable<CourseEntity>> include 
            = query => query.Include(c => c.Students);
        
        if (await repository.ExistsAsync(entity))
        {
            CourseEntity updatedEntity = await repository.UpdateAsync(entity, include);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            CourseEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }
}