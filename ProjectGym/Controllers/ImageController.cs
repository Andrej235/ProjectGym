using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase, IReadController<ExerciseImage, ImageDTO, int>
    {
        public IReadService<ExerciseImage> ReadService { get; }
        public IEntityMapper<ExerciseImage, ImageDTO> Mapper { get; }
        public ImageController(IReadService<ExerciseImage> readService, IEntityMapper<ExerciseImage, ImageDTO> mapper)
        {
            ReadService = readService;
            Mapper = mapper;
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
