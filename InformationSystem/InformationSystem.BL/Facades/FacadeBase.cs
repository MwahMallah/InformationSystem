using System.Collections;
using System.Data.Common;
using System.Reflection;
using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.Repositories;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public abstract class
    FacadeBase<TEntity, TDetailModel, TListModel, TEntityMapper>(
        IUnitOfWorkFactory unitOfWorkFactory,
        IModelMapper<TEntity, TDetailModel, TListModel> modelMapper)
    : IFacade<TEntity, TDetailModel, TListModel>
    where TEntity : class, IEntity
    where TDetailModel : class, IModel
    where TListModel : IModel
    where TEntityMapper : IEntityMapper<TEntity>, new()
{
    protected readonly IModelMapper<TEntity, TDetailModel, TListModel> ModelMapper = modelMapper;
    protected readonly IUnitOfWorkFactory UnitOfWorkFactory = unitOfWorkFactory;
    protected virtual string IncludesNavigationPathDetail => string.Empty;
    
    public async Task DeleteAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        try
        {
            uow.GetRepository<TEntity, TEntityMapper>().Delete(id);
            await uow.CommitAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException e)
        {
            throw new InvalidOperationException("Entity deletion failed.", e);
        }
    }

    public virtual async Task<IEnumerable<TListModel>> GetAsync()
    {
        await using var uow = unitOfWorkFactory.Create();
        var entities = await uow.GetRepository<TEntity, TEntityMapper>()
            .Get().ToListAsync();
        
        return ModelMapper.MapToListModel(entities);
    }

    public virtual async Task<TDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        IQueryable<TEntity> query = uow.GetRepository<TEntity, TEntityMapper>()
            .Get();
        
        if (!string.IsNullOrWhiteSpace(IncludesNavigationPathDetail))
        {
            query = query.Include(IncludesNavigationPathDetail);
        }

        TEntity? entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }

    public virtual async Task<TDetailModel> SaveAsync(TDetailModel model)
    {
        TDetailModel result;
        GuardCollectionsAreNotSet(model);

        TEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        IRepository<TEntity> repository = uow.GetRepository<TEntity, TEntityMapper>();

        if (await repository.ExistsAsync(entity))
        {
            TEntity updatedEntity = await repository.UpdateAsync(entity);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            TEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }
    
    protected async Task AddEntitiesToCollectionAsync<TCollectionEntity, TCollectionEntityMapper>
        (ICollection<TCollectionEntity> collection, IEnumerable<Guid> ids, IUnitOfWork uow)
        where TCollectionEntity: class, IEntity
        where TCollectionEntityMapper: IEntityMapper<TCollectionEntity>, new()
    {
        var repository = uow.GetRepository<TCollectionEntity, TCollectionEntityMapper>();

        foreach (var id in ids)
        {
            var entity = await repository.Get().SingleOrDefaultAsync(e=>e.Id==id);
            
            if (entity != null)
            {
                collection.Add(entity);
            }
            else
            {
                throw new InvalidOperationException($"Entity with ID {id} does not exist.");
            }
        }
    }
    
    protected async Task<TNavigationalPropertyEntity> GetEntityOrThrowAsync<TNavigationalPropertyEntity, TNavigationalPropertyEntityMapper>
        (Guid? id, IUnitOfWork uow)
        where TNavigationalPropertyEntity: class, IEntity
        where TNavigationalPropertyEntityMapper: IEntityMapper<TNavigationalPropertyEntity>, new()
    {
        var repository = uow.GetRepository<TNavigationalPropertyEntity, TNavigationalPropertyEntityMapper>();
        var entity = await repository.Get().SingleOrDefaultAsync(p=>p.Id == id);
        
        if (entity != null)
        {
            return entity;
        }

        throw new InvalidOperationException($"Entity with ID {id} does not exist.");
    }
    
    
    private static void GuardCollectionsAreNotSet(TDetailModel model)
    {
        IEnumerable<PropertyInfo> collectionProperties = model
            .GetType()
            .GetProperties()
            .Where(i => typeof(ICollection).IsAssignableFrom(i.PropertyType));

        foreach (PropertyInfo collectionProperty in collectionProperties)
        {
            if (collectionProperty.GetValue(model) is ICollection { Count: > 0 })
            {
                throw new InvalidOperationException(
                    "Current BL and DAL infrastructure disallows insert or update of models with adjacent collections.");
            }
        }
    }
}