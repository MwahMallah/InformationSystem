using InformationSystem.BL.Facades.Interfaces;
using InformationSystem.BL.Mappers;
using InformationSystem.BL.Mappers.Interfaces;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class EvaluationFacade(IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<EvaluationEntity, EvaluationDetailModel, EvaluationListModel> modelMapper) 
    : FacadeBase<EvaluationEntity, EvaluationDetailModel, 
            EvaluationListModel, EvaluationEntityMapper>(unitOfWorkFactory, modelMapper), IEvaluationFacade
{
    public override async Task<EvaluationDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<EvaluationEntity, EvaluationEntityMapper>();
        
        IQueryable<EvaluationEntity> query = repository.Get()
            .Include(e=>e.Student)
            .Include(e=>e.Activity)
            .ThenInclude(a => a.Course);
        
        var entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }
    
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