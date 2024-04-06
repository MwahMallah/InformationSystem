using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public interface IModelMapper<TEntity, TDetailModel, out TListModel> 
    where TEntity : IEntity
    where TDetailModel: IModel
    where TListModel: IModel
{
    public TListModel MapToListModel(TEntity entity);
    public IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities);
    public TDetailModel MapToDetailModel(TEntity entity);
    public TEntity MapToEntity(TDetailModel model);
}