using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public class EquipmentController(ICreateService<Equipment> createService, IUpdateService<Equipment> updateService, IDeleteService<Equipment> deleteService, IReadService<Equipment> readService, IEntityMapper<Equipment, EquipmentDTO> mapper) : RepositoryController<Equipment, EquipmentDTO>(createService, updateService, deleteService, readService, mapper) { }
}
