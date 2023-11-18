using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    [Route("api/equipment")]
    [ApiController]
    public class EquipmentController(IEntityMapperSync<Equipment, EquipmentDTO> mapper,
                                     IReadService<Equipment> readService,
                                     ICreateService<Equipment, int> createService,
                                     IUpdateService<Equipment> updateService,
                                     IDeleteService<Equipment> deleteService) : ControllerBase,
                                                                                IReadController<Equipment, EquipmentDTO, int>,
                                                                                ICreateController<Equipment, EquipmentDTO, int>,
                                                                                IUpdateController<Equipment, EquipmentDTO>,
                                                                                IDeleteController<Equipment, int>
    {
        public IEntityMapperSync<Equipment, EquipmentDTO> Mapper { get; } = mapper;
        public IReadService<Equipment> ReadService { get; } = readService;
        public ICreateService<Equipment, int> CreateService { get; } = createService;
        public IUpdateService<Equipment> UpdateService { get; } = updateService;
        public IDeleteService<Equipment> DeleteService { get; } = deleteService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
        {
            try
            {
                return Ok(Mapper.Map(await ReadService.Get(p => p.Id == id, include)));
            }
            catch (NullReferenceException)
            {
                return NotFound($"Entity with id {id} was not found.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q) => Ok((await ReadService.Get(q, offset, limit, include)).Select(Mapper.Map));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipmentDTO entityDTO)
        {
            try
            {
                int newEntityId = await CreateService.Add(Mapper.Map(entityDTO));
                if (newEntityId != default)
                    return Ok(newEntityId);
                else
                    return BadRequest("Entity already exists.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EquipmentDTO updatedEntity)
        {
            try
            {
                var entity = Mapper.Map(updatedEntity);
                await UpdateService.Update(entity);
                return Ok("Successfully updated entity");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{primaryKey}")]
        public async Task<IActionResult> Delete(int primaryKey)
        {
            try
            {
                await DeleteService.DeleteFirst(x => x.Id == primaryKey);
                return Ok($"Entity with primary key {primaryKey} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}
