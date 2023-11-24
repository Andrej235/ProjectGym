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
    [Route("api/musclegroup")]
    public class MuscleGroupController(ICreateService<MuscleGroup, int> createService,
                                       IReadService<MuscleGroup> readService,
                                       IEntityMapperSync<MuscleGroup, MuscleGroupDTO> mapper,
                                       IUpdateService<MuscleGroup> updateService,
                                       IDeleteService<MuscleGroup> deleteService) :
        ControllerBase,
        ICreateController<MuscleGroup, MuscleGroupDTO, int>,
        IReadController<MuscleGroup, MuscleGroupDTO, int>,
        IUpdateController<MuscleGroup, MuscleGroupDTO>,
        IDeleteController<MuscleGroup, int>
    {
        public ICreateService<MuscleGroup, int> CreateService { get; } = createService;
        public IReadService<MuscleGroup> ReadService { get; } = readService;
        public IUpdateService<MuscleGroup> UpdateService { get; } = updateService;
        public IDeleteService<MuscleGroup> DeleteService { get; } = deleteService;
        public IEntityMapperSync<MuscleGroup, MuscleGroupDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MuscleGroupDTO entityDTO)
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
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        public async Task<IActionResult> Delete(int primaryKey)
        {
            try
            {
                await DeleteService.DeleteFirst(x => x.Id == primaryKey);
                return Ok("Successfully deleted entity.");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
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
        public async Task<IActionResult> Update([FromBody] MuscleGroupDTO updatedEntity)
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
