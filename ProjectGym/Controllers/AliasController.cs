using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/alias")]
    [ApiController]
    public class AliasController(ICreateService<Alias, int> createService,
                                 IReadService<Alias> readService,
                                 IEntityMapperSync<Alias, ExerciseAliasDTO> mapper) : ControllerBase, IReadController<Alias, ExerciseAliasDTO, int>, ICreateController<Alias, ExerciseAliasDTO, int>
    {
        public ICreateService<Alias, int> CreateService { get; } = createService;
        public IReadService<Alias> ReadService { get; } = readService;
        public IEntityMapperSync<Alias, ExerciseAliasDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExerciseAliasDTO entityDTO)
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
    }
}
