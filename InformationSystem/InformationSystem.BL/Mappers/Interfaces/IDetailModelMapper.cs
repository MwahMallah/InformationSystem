using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Mappers.Interfaces;

public interface IDetailModelMapper<TEntity, out TDetailModel> 
    where TEntity : IEntity
    where TDetailModel: IModel
{
    public TDetailModel MapToDetailModel(TEntity entity);

}