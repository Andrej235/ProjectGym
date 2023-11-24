using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class SetController(ICreateService<Set> createService, IUpdateService<Set> updateService, IDeleteService<Set> deleteService, IReadService<Set> readService, IEntityMapper<Set, SetDTO> mapper) : RepositoryController<Set, SetDTO>(createService, updateService, deleteService, readService, mapper) { }
}