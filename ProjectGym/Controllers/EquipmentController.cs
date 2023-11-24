using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [Route("api/equipment")]
    [ApiController]
    public class EquipmentController(IEntityMapperSync<Equipment, EquipmentDTO> mapper,
                                     IReadService<Equipment> readService,
                                     ICreateService<Equipment> createService,
                                     IUpdateService<Equipment> updateService,
                                     IDeleteService<Equipment> deleteService) :
        ControllerBase,
        IReadController<Equipment, EquipmentDTO>,
        ICreateController<Equipment, EquipmentDTO>,
        IUpdateController<Equipment, EquipmentDTO>,
        IDeleteController<Equipment>
    {
        public IEntityMapperSync<Equipment, EquipmentDTO> Mapper { get; } = mapper;
        public IReadService<Equipment> ReadService { get; } = readService;
        public ICreateService<Equipment> CreateService { get; } = createService;
        public IUpdateService<Equipment> UpdateService { get; } = updateService;
        public IDeleteService<Equipment> DeleteService { get; } = deleteService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery] string? include)
        {
            try
            {
                return Ok(Mapper.Map(await ReadService.Get(id, include)));
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            try
            {
                var entities = (await ReadService.Get(q, offset, limit, include)).Select(Mapper.Map);
                return Ok(entities);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipmentDTO entityDTO)
        {
            try
            {
                var newEntityId = await CreateService.Add(Mapper.Map(entityDTO));
                if (newEntityId != default)
                    return Ok(newEntityId);
                else
                    return BadRequest("Entity already exists.");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EquipmentDTO updatedEntity)
        {
            try
            {
                await UpdateService.Update(Mapper.Map(updatedEntity));
                return Ok("Successfully updated entity");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpDelete("{primaryKey}")]
        public async Task<IActionResult> Delete(string primaryKey)
        {
            try
            {
                await DeleteService.Delete(primaryKey);
                return Ok("Successfully deleted entity.");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }
    }
}
