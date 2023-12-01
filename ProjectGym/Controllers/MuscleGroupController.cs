using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class MuscleGroupController(ICreateService<MuscleGroup> createService, IUpdateService<MuscleGroup> updateService, IDeleteService<MuscleGroup> deleteService, IReadService<MuscleGroup> readService, IEntityMapper<MuscleGroup, MuscleGroupDTO> mapper) : RepositoryController<MuscleGroup, MuscleGroupDTO>(createService, updateService, deleteService, readService, mapper) { }
}