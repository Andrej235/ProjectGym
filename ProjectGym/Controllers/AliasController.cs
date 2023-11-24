using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class AliasController(ICreateService<Alias> createService, IUpdateService<Alias> updateService, IDeleteService<Alias> deleteService, IReadService<Alias> readService, IEntityMapper<Alias, AliasDTO> mapper) : RepositoryController<Alias, AliasDTO>(createService, updateService, deleteService, readService, mapper) { }
}
