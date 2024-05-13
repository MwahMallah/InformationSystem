using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;

namespace InformationSystem.BL.Facades.Interfaces;

public interface IFacade<TEntity, TDetailModel, TListModel>
    where TEntity: class, IEntity
    where TDetailModel: class, IModel
    where TListModel: IModel
{
    public Task DeleteAsync(Guid id);
    public Task<IEnumerable<TListModel>> GetAsync();
    public Task<TDetailModel?> GetAsync(Guid id);
    public Task<TDetailModel> SaveAsync(TDetailModel model);
}