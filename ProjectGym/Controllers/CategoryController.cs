using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase, IReadController<ExerciseCategory, CategoryDTO, int>
    {
        public CategoryController(IReadService<ExerciseCategory> readService, IEntityMapper<ExerciseCategory, CategoryDTO> mapper)
        {
            ReadService = readService;
            Mapper = mapper;
        }
        public IReadService<ExerciseCategory> ReadService { get; }
        public IEntityMapper<ExerciseCategory, CategoryDTO> Mapper { get; }

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
