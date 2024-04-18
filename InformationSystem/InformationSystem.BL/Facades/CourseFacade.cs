using InformationSystem.BL.Mappers;
using InformationSystem.BL.Models;
using InformationSystem.DAL.Entities;
using InformationSystem.DAL.Mappers;
using InformationSystem.DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace InformationSystem.BL.Facades;

public class CourseFacade(
    IUnitOfWorkFactory unitOfWorkFactory, 
    IModelMapper<CourseEntity, CourseDetailModel, CourseListModel> modelMapper) 
    : FacadeBase<CourseEntity, CourseDetailModel, CourseListModel, CourseEntityMapper>(unitOfWorkFactory, modelMapper)
{
    public override async Task<CourseDetailModel?> GetAsync(Guid id)
    {
        await using var uow = unitOfWorkFactory.Create();
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        var query = repository.Get()
            .Include(c=>c.Students)
            .Include(c=>c.Activities);
        
        var entity = await query.SingleOrDefaultAsync(e => e.Id == id);
        return entity is null
            ? null
            : ModelMapper.MapToDetailModel(entity);
    }
    
    public override async Task<CourseDetailModel> SaveAsync(CourseDetailModel model)
    {
        var entity = ModelMapper.MapToEntity(model);
        var uow = UnitOfWorkFactory.Create();
        
        var repository = uow.GetRepository<CourseEntity, CourseEntityMapper>();
        
        await AddStudentsToCourse(entity, model.Students.Select(s => s.Id), uow);
        await AddActivitiesToCourse(entity, model.Activities.Select(a => a.Id), uow);
        
        Func<IQueryable<CourseEntity>, IQueryable<CourseEntity>> include 
            = query => query.Include(c => c.Students)
                            .Include(c=> c.Activities);
        
        if (await repository.ExistsAsync(entity))
        {
            entity = await repository.UpdateAsync(entity, include);
        }
        else
        {
            entity.Id = Guid.NewGuid();
            entity = await repository.InsertAsync(entity);
        }
        
        var result = ModelMapper.MapToDetailModel(entity);
        await uow.CommitAsync();
        return result;
    }

    private async Task AddStudentsToCourse(CourseEntity entity, IEnumerable<Guid> studentsIds, IUnitOfWork uow)
    {
        var studentRepository = uow.GetRepository<StudentEntity, StudentEntityMapper>();

        foreach (var studentId in studentsIds)
        {
            var student = await studentRepository.Get().SingleOrDefaultAsync(c=>c.Id==studentId);
            
            if (student != null)
            {
                entity.Students.Add(student);
            }
            else
            {
                throw new InvalidOperationException($"Student with ID {studentId} does not exist.");
            }
        }
    }
    
    private async Task AddActivitiesToCourse(CourseEntity entity, IEnumerable<Guid> activityIds, IUnitOfWork uow)
    {
        var activityRepository = uow.GetRepository<ActivityEntity, ActivityEntityMapper>();

        foreach (var activityId in activityIds)
        {
            var activity = await activityRepository.Get().SingleOrDefaultAsync(a=>a.Id==activityId);
            
            if (activity != null)
            {
                entity.Activities.Add(activity);
            }
            else
            {
                throw new InvalidOperationException($"Activity with ID {activityId} does not exist.");
            }
        }
    }
}