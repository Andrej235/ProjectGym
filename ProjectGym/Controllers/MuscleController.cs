using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    [Route("api/muscle")]
    [ApiController]
    public class MuscleController : ControllerBase
    {
        private readonly IReadService<Muscle> readService;
        private readonly IEntityMapper<Muscle, MuscleDTO> mapper;

        public MuscleController(IReadService<Muscle> readService, IEntityMapper<Muscle, MuscleDTO> mapper)
        {
            this.readService = readService;
            this.mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
        {
            try
            {
                return Ok(mapper.MapEntity(await readService.Get(p => p.Id == id, include)));
            }
            catch (Exception)
            {
                return NotFound($"Exercise with id {id} was not found.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            return Ok((await readService.Get(q, offset, limit, include)).Select(mapper.MapEntity));
        }
    }
}
