using InformationSystem.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata;
using IModel = InformationSystem.BL.Models.IModel;

namespace InformationSystem.BL.Mappers;

public abstract class ListModelMapperBase<TEntity, TListModel>
    : IListModelMapper<TEntity, TListModel>
    where TEntity: IEntity
    where TListModel: IModel
{
    public abstract TListModel MapToListModel(TEntity entity);

    public IEnumerable<TListModel> MapToListModel(IEnumerable<TEntity> entities)
    {
        return entities.Select(MapToListModel);
    }
}