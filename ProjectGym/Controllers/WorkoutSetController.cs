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
    [Route("api/workoutset")]
    public class WorkoutSetController(IReadService<WorkoutSet> readService,
                                      ICreateService<WorkoutSet, Guid> createService,
                                      IEntityMapperSync<WorkoutSet, WorkoutSetDTO> mapper,
                                      IUpdateService<WorkoutSet> updateService,
                                      IDeleteService<WorkoutSet> deleteService)
        : ControllerBase,
        ICreateController<WorkoutSet, WorkoutSetDTO, Guid>,
        IReadController<WorkoutSet, WorkoutSetDTO, Guid>,
        IUpdateController<WorkoutSet, WorkoutSetDTO>,
        IDeleteController<WorkoutSet, Guid>
    {
        public IReadService<WorkoutSet> ReadService { get; } = readService;
        public ICreateService<WorkoutSet, Guid> CreateService { get; } = createService;
        public IUpdateService<WorkoutSet> UpdateService { get; } = updateService;
        public IDeleteService<WorkoutSet> DeleteService { get; } = deleteService;
        public IEntityMapperSync<WorkoutSet, WorkoutSetDTO> Mapper { get; } = mapper;


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkoutSetDTO entityDTO)
        {
            try
            {
                Guid newEntityId = await CreateService.Add(Mapper.Map(entityDTO));
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
        public async Task<IActionResult> Delete(Guid primaryKey)
        {
            try
            {
                await DeleteService.DeleteFirst(x => x.Id == primaryKey);
                return Ok("Successfully deleted entity");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id, [FromQuery] string? include)
        {
            try
            {
                return Ok(Mapper.Map(await ReadService.Get(x => x.Id == id, include)));
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
        public async Task<IActionResult> Update([FromBody] WorkoutSetDTO updatedEntity)
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
