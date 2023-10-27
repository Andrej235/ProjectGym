using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/alias")]
    [ApiController]
    public class AliasController : ControllerBase, IReadController<ExerciseAlias, ExerciseAliasDTO>
    {
        public IReadService<ExerciseAlias> ReadService { get; }
        public IEntityMapper<ExerciseAlias, ExerciseAliasDTO> Mapper { get; }
        public AliasController(IReadService<ExerciseAlias> readService, IEntityMapper<ExerciseAlias, ExerciseAliasDTO> mapper)
        {
            ReadService = readService;
            Mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
        {
            try
            {
                return Ok(Mapper.MapEntity(await ReadService.Get(p => p.Id == id, include)));
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
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q) => Ok((await ReadService.Get(q, offset, limit, include)).Select(Mapper.MapEntity));
    }
}
