using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public abstract class ModelMapperBase<TEntity, TDetailModel, TListModel> 
    : IModelMapper<TEntity, TDetailModel, TListModel> 
    where TEntity : IEntity 
    where TDetailModel : IModel 
    where TListModel : IModel
{
    public abstract TListModel MapToListModel(TEntity entity);
    public IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
    {
        return entities.Select(MapToListModel);
    }
    public abstract TDetailModel MapToDetailModel(TEntity entity);
    public abstract TEntity MapToEntity(TDetailModel model);
}