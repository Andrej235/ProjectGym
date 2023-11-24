using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/alias")]
    [ApiController]
    public class AliasController(ICreateService<Alias> createService,
                                 IReadService<Alias> readService,
                                 IEntityMapperSync<Alias, ExerciseAliasDTO> mapper,
                                 IDeleteService<Alias> deleteService) :
        ControllerBase,
        IReadController<Alias, ExerciseAliasDTO>,
        ICreateController<Alias, ExerciseAliasDTO>,
        IDeleteController<Alias>
    {
        public ICreateService<Alias> CreateService { get; } = createService;
        public IReadService<Alias> ReadService { get; } = readService;
        public IDeleteService<Alias> DeleteService { get; } = deleteService;
        public IEntityMapperSync<Alias, ExerciseAliasDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExerciseAliasDTO entityDTO)
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
    }
}
