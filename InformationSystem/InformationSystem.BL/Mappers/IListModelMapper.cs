using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public interface IListModelMapper<TEntity, out TListModel> 
    where TEntity : IEntity
    where TListModel: IModel
{
    public TListModel MapToListModel(TEntity entity);
    public IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities);
}