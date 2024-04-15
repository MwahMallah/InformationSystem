using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers;

public interface IModelMapper<TEntity, TDetailModel, out TListModel>
    : IListModelMapper<TEntity, TListModel>, 
        IDetailModelMapper<TEntity, TDetailModel>
    where TEntity : IEntity
    where TDetailModel: IModel
    where TListModel: IModel
{
    public TEntity MapToEntity(TDetailModel model);
}