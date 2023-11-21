using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/workout")]
    public class WorkoutController(IReadService<Workout> readService, ICreateService<Workout, Guid> createService, IEntityMapperSync<Workout, WorkoutDTO> mapper) : ControllerBase, ICreateController<Workout, WorkoutDTO, Guid>, IReadController<Workout, WorkoutDTO, Guid>
    {
        public IReadService<Workout> ReadService { get; } = readService;
        public ICreateService<Workout, Guid> CreateService { get; } = createService;
        public IEntityMapperSync<Workout, WorkoutDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkoutDTO entityDTO)
        {
            try
            {
                return Ok(await CreateService.Add(Mapper.Map(entityDTO)));
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
    }
}
