using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IActivityModelMapper modelMapper) 
    : FacadeBase<ActivityEntity, ActivityDetailModel, 
        ActivityListModel, ActivityEntityMapper>(unitOfWorkFactory, modelMapper), IActivityFacade
{
    public override async Task<IEnumerable<ActivityListModel>> GetAsync()
    {
        return await GetAsync(null, null);
    }

    public async Task<IEnumerable<ActivityListModel>> GetAsync(
        string? searchQuery = null, string? orderQuery = null, bool isAscending = true)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        IQueryable<ActivityEntity> query = repository.Get()
            .Include(a => a.Course);

        if (!string.IsNullOrEmpty(searchQuery))
        {
            // Finds Activity that has matching course Abbreviation or Description
            query = query.Where(a => 
                (a.Description != null && a.Description.StartsWith(searchQuery.ToLower())) || 
                (a.Course != null && a.Course.Abbreviation.StartsWith(searchQuery.ToLower())));
        }

        if (!string.IsNullOrEmpty(orderQuery))
        {
            query = orderQuery.ToLower() switch
            {
                "course_abbreviation" => isAscending 
                    ? query.OrderBy(a => a.Course.Abbreviation) 
                    : query.OrderByDescending(a => a.Course.Abbreviation),
                "max_points" => isAscending
                    ? query.OrderBy(a => a.MaxPoints)
                    : query.OrderByDescending(a => a.MaxPoints),
                "start_time" => isAscending 
                    ? query.OrderBy(a => a.StartTime) 
                    : query.OrderByDescending(a => a.StartTime),
                "finish_time" => isAscending
                    ? query.OrderBy(a => a.FinishTime)
                    : query.OrderByDescending(a => a.FinishTime),
                _ => query
            };
        }
        
        var entities = await query.ToListAsync();
        return modelMapper.MapToListModel(entities);
    }

    public async Task<IEnumerable<ActivityListModel>> GetFromCourseAsync(
        Guid courseId, DateTime? startTime = null, DateTime? finishTime = null)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();
        
        var query = repository.Get()
            .Include(a => a.Course)
            .Where(a => a.CourseId == courseId);
        
        if (startTime.HasValue)
        {
            query = query.Where(a => a.StartTime >= startTime);
        }

        if (finishTime.HasValue)
        {
            query = query.Where(a => a.FinishTime <= finishTime);
        }

        var entities = await query.ToListAsync();
        return modelMapper.MapToListModel(entities);
    }
    
    public override async Task<ActivityDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();
        
        var query = repository.Get()
                            .Include(a=>a.Course)
                            .Include(a => a.Evaluations)
                            .ThenInclude(e => e.Student);
        
        var entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    public override async Task<ActivityDetailModel> SaveAsync(ActivityDetailModel model)
    {
        ActivityEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();
        entity.Course = await GetEntityOrThrowAsync<CourseEntity, CourseEntityMapper>(model.CourseId, uow);
        
        Func<IQueryable<ActivityEntity>, IQueryable<ActivityEntity>> include 
            = query => query.Include(a=>a.Course);
        
        ActivityDetailModel result;
        if (await repository.ExistsAsync(entity))
        {
            ActivityEntity updatedEntity = await repository.UpdateAsync(entity, include);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            ActivityEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }
}