using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/musclegroup")]
    public class MuscleGroupController : ControllerBase, ICreateController<MuscleGroup, MuscleGroupDTO, int>, IReadController<MuscleGroup, MuscleGroupDTO, int>
    {
        public IEntityMapperSync<MuscleGroup, MuscleGroupDTO> Mapper { get; }
        public ICreateService<MuscleGroup, int> CreateService { get; }
        public IReadService<MuscleGroup> ReadService { get; }

        public MuscleGroupController(ICreateService<MuscleGroup, int> createService, IReadService<MuscleGroup> readService, IEntityMapperSync<MuscleGroup, MuscleGroupDTO> mapper)
        {
            CreateService = createService;
            ReadService = readService;
            Mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MuscleGroupDTO entityDTO)
        {
            try
            {
                return Ok(await CreateService.Add(Mapper.Map(entityDTO)));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

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
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            try
            {
                return Ok((await ReadService.Get(q, offset, limit, include)).Select(Mapper.Map));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}
