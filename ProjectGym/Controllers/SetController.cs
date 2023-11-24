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
    [ApiController]
    [Route("api/set")]
    public class SetController(IReadService<Set> readService,
                               ICreateService<Set> createService,
                               IEntityMapperSync<Set, SetDTO> mapper,
                               IUpdateService<Set> updateService,
                               IDeleteService<Set> deleteService) : 
        ControllerBase, 
        ICreateController<Set, SetDTO>,
        IReadController<Set, SetDTO>, 
        IUpdateController<Set, SetDTO>, 
        IDeleteController<Set>
    {
        public IReadService<Set> ReadService { get; } = readService;
        public ICreateService<Set> CreateService { get; } = createService;
        public IUpdateService<Set> UpdateService { get; } = updateService;
        public IDeleteService<Set> DeleteService { get; } = deleteService;
        public IEntityMapperSync<Set, SetDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SetDTO entityDTO)
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SetDTO updatedEntity)
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
    }
}
