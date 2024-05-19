using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class StudentFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IStudentModelMapper modelMapper) 
    : FacadeBase<StudentEntity, StudentDetailModel, StudentListModel, StudentEntityMapper>
        (unitOfWorkFactory, modelMapper), IStudentFacade
{
    public override async Task<IEnumerable<StudentListModel>> GetAsync()
    {
        return await GetAsync(null, null);
    }
    
    public async Task<IEnumerable<StudentListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
    
        var query = repository.Get();
    
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(c => c.FirstName.ToLower().StartsWith(searchQuery)
                                     || c.LastName.ToLower().StartsWith(searchQuery)
                                     || c.Group.ToLower().StartsWith(searchQuery));
        }
    
        if (!string.IsNullOrEmpty(orderQuery))
        {
            query = orderQuery.ToLower() switch
            {
                "name" => isAscending 
                    ? query.OrderBy(c => c.LastName + " " + c.FirstName) 
                    : query.OrderByDescending(c => c.LastName + " " + c.FirstName),
                "current_year" => isAscending
                    ? query.OrderBy(c => c.StartYear)
                    : query.OrderByDescending(c => c.StartYear),
                "group" => isAscending 
                    ? query.OrderBy(c => c.Group) 
                    : query.OrderByDescending(c => c.Group),
                _ => query
            };
        }
            
        var entities = await query.ToListAsync();
        return modelMapper.MapToListModel(entities);
    }
    
    public async Task<IEnumerable<StudentListModel>> GetCourseStudentsAsync(Guid courseId, string? filterText)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
        
        IQueryable<StudentEntity> query = repository.Get().Include(s => s.Courses);
        query = query.Where(s => s.Courses.Any(c => c.Id == courseId));

        if (!string.IsNullOrEmpty(filterText))
        {
            query = query.Where(s => s.FirstName.ToLower().StartsWith(filterText)
                                     || s.LastName.ToLower().StartsWith(filterText)
                                     || s.Group.ToLower().StartsWith(filterText));
        }
        
        var entities = await query.ToListAsync();
        return modelMapper.MapToListModel(entities);
    }
    
    public override async Task<StudentDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<StudentEntity, StudentEntityMapper>();
        
        var query = repository.Get()
            .Include(s=>s.Courses)
            .ThenInclude(c=>c.Activities)
            .ThenInclude(a=>a.Evaluations);
        
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
        
        //add courses to student entity
        await AddEntitiesToCollectionAsync<CourseEntity, CourseEntityMapper>
            (entity.Courses, model.Courses.Select(c => c.Id), uow);
        
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