using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class MuscleController(ICreateService<Muscle> createService, IUpdateService<Muscle> updateService, IDeleteService<Muscle> deleteService, IReadService<Muscle> readService, IEntityMapper<Muscle, MuscleDTO> mapper) : RepositoryController<Muscle, MuscleDTO>(createService, updateService, deleteService, readService, mapper) { }
}