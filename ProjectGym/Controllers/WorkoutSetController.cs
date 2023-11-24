using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class WorkoutSetController(ICreateService<WorkoutSet> createService, IUpdateService<WorkoutSet> updateService, IDeleteService<WorkoutSet> deleteService, IReadService<WorkoutSet> readService, IEntityMapper<WorkoutSet, WorkoutSetDTO> mapper) : RepositoryController<WorkoutSet, WorkoutSetDTO>(createService, updateService, deleteService, readService, mapper) { }
}