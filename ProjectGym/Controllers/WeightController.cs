using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class WeightController(ICreateService<PersonalExerciseWeight> createService, IUpdateService<PersonalExerciseWeight> updateService, IDeleteService<PersonalExerciseWeight> deleteService, IReadService<PersonalExerciseWeight> readService, IEntityMapper<PersonalExerciseWeight, PersonalExerciseWeightDTO> mapper) : RepositoryController<PersonalExerciseWeight, PersonalExerciseWeightDTO>(createService, updateService, deleteService, readService, mapper) { }
}