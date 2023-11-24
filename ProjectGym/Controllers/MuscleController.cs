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
    [Route("api/muscle")]
    [ApiController]
    public class MuscleController(IReadService<Muscle> readService,
                                  IEntityMapperSync<Muscle, MuscleDTO> mapper,
                                  ICreateService<Muscle, int> createService,
                                  IUpdateService<Muscle> updateService,
                                  IDeleteService<Muscle> deleteService) :
        ControllerBase,
        IReadController<Muscle, MuscleDTO, int>,
        ICreateController<Muscle, MuscleDTO, int>,
        IUpdateController<Muscle, MuscleDTO>,
        IDeleteController<Muscle, int>
    {
        public IReadService<Muscle> ReadService { get; } = readService;
        public IEntityMapperSync<Muscle, MuscleDTO> Mapper { get; } = mapper;
        public IUpdateService<Muscle> UpdateService { get; } = updateService;
        public IDeleteService<Muscle> DeleteService { get; } = deleteService;
        public ICreateService<Muscle, int> CreateService { get; } = createService;

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MuscleDTO entityDTO)
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] MuscleDTO updatedEntity)
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

        [HttpDelete]
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
    }
}
