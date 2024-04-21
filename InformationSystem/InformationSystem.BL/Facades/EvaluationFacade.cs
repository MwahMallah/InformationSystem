using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class EvaluationFacade(IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<EvaluationEntity, EvaluationDetailModel, EvaluationListModel> modelMapper) 
    : FacadeBase<EvaluationEntity, EvaluationDetailModel, EvaluationListModel, EvaluationEntityMapper>
        (unitOfWorkFactory, modelMapper)
{
    public override async Task<EvaluationDetailModel> SaveAsync(EvaluationDetailModel model)
    {
        EvaluationDetailModel result;
        EvaluationEntity entity = ModelMapper.MapToEntity(model);

        IUnitOfWork uow = UnitOfWorkFactory.Create();
        var repository = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();
        
        entity.Student = await GetEntityOrThrowAsync<StudentEntity, StudentEntityMapper>(model.StudentId, uow);
        entity.Activity = await GetEntityOrThrowAsync<ActivityEntity, ActivityEntityMapper>(model.ActivityId, uow);
        
        Func<IQueryable<EvaluationEntity>, IQueryable<EvaluationEntity>> include 
            = query => query.Include(e=>e.Activity)
                            .Include(e => e.Student);
        
        if (await repository.ExistsAsync(entity))
        {
            EvaluationEntity updatedEntity = await repository.UpdateAsync(entity, include);
            result = ModelMapper.MapToDetailModel(updatedEntity);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            EvaluationEntity insertedEntity = await repository.InsertAsync(entity);
            result = ModelMapper.MapToDetailModel(insertedEntity);
        }

        await uow.CommitAsync();

        return result;
    }
}