using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class ActivityFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<ActivityEntity, ActivityDetailModel, ActivityListModel> modelMapper) 
    : FacadeBase<ActivityEntity, ActivityDetailModel, 
        ActivityListModel, ActivityEntityMapper>(unitOfWorkFactory, modelMapper)
{
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