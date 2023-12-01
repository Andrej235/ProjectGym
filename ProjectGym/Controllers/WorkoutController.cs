using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class WorkoutController(ICreateService<Workout> createService, IUpdateService<Workout> updateService, IDeleteService<Workout> deleteService, IReadService<Workout> readService, IEntityMapper<Workout, WorkoutDTO> mapper) : RepositoryController<Workout, WorkoutDTO>(createService, updateService, deleteService, readService, mapper) { }
}
