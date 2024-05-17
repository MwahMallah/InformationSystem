using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class CourseFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    ICourseModelMapper modelMapper) 
    : FacadeBase<CourseEntity, CourseDetailModel, 
            CourseListModel, CourseEntityMapper>(unitOfWorkFactory, modelMapper), ICourseFacade
{
    public override async Task<IEnumerable<CourseListModel>> GetAsync()
    {
        return await GetAsync(null, null);
    }
    
    public async Task<IEnumerable<CourseListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        IQueryable<CourseEntity> query = repository.Get();
        
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(c => c.Abbreviation.StartsWith(searchQuery)
                        || c.Name.ToLower().Contains(searchQuery.ToLower()));
        }

        if (!string.IsNullOrEmpty(orderQuery))
        {
            query = orderQuery.ToLower() switch
            {
                "name" => isAscending 
                    ? query.OrderBy(c => c.Name) 
                    : query.OrderByDescending(c => c.Name),
                "abbreviation" => isAscending
                    ? query.OrderBy(c => c.Abbreviation)
                    : query.OrderByDescending(c => c.Abbreviation),
                "credits" => isAscending 
                    ? query.OrderBy(c => c.Credits) 
                    : query.OrderByDescending(c => c.Credits),
                "max_students" => isAscending
                    ? query.OrderBy(c => c.MaxStudents)
                    : query.OrderByDescending(c => c.MaxStudents),
                _ => query
            };
        }
        
        var entities = await query.ToListAsync();
        return modelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<CourseListModel>> GetStudentsCoursesAsync(Guid studentId, string? filterText)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        IQueryable<CourseEntity> query = repository.Get().Include(c => c.Students);
        query = query.Where(c => c.Students.Any(s => s.Id == studentId));

        if (!string.IsNullOrEmpty(filterText))
        {
            query = query.Where(c => c.Abbreviation.StartsWith(filterText)
                                     || c.Name.ToLower().Contains(filterText.ToLower()));
        }
        
        var entities = await query.ToListAsync();
        return modelMapper.MapToListModel(entities);
    }
    
    public override async Task<CourseDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        IQueryable<CourseEntity> query = repository.Get()
            .Include(c=>c.Students)
            .Include(c=>c.Activities);
        
        var entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }
    
    public override async Task<CourseDetailModel> SaveAsync(CourseDetailModel model)
    {
        var entity = ModelMapper.MapToEntity(model);
        var uow = UnitOfWorkFactory.Create();
        
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        //Add students to course entity
        await AddEntitiesToCollectionAsync<StudentEntity, StudentEntityMapper>
            (entity.Students, model.Students.Select(s => s.Id), uow);
        //add activities to course entity
        await AddEntitiesToCollectionAsync<ActivityEntity, ActivityEntityMapper>
            (entity.Activities, model.Activities.Select(a => a.Id), uow);
        
        Func<IQueryable<CourseEntity>, IQueryable<CourseEntity>> include 
            = query => query.Include(c => c.Students)
                            .Include(c=> c.Activities);
        
        if (await repository.ExistsAsync(entity))
        {
            entity = await repository.UpdateAsync(entity, include);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            entity = await repository.InsertAsync(entity);
        }
        
        var result = ModelMapper.MapToDetailModel(entity);
        await uow.CommitAsync();
        return result;
    }
}