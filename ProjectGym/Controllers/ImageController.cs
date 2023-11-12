using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController : ControllerBase, IReadController<ExerciseImage, ImageDTO, int>, ICreateController<ExerciseImage, ImageDTO, int>
    {
        public IReadService<ExerciseImage> ReadService { get; }
        public IEntityMapperSync<ExerciseImage, ImageDTO> Mapper { get; }
        public ICreateService<ExerciseImage, int> CreateService { get; }

        public ImageController(IReadService<ExerciseImage> readService, IEntityMapperSync<ExerciseImage, ImageDTO> mapper, ICreateService<ExerciseImage, int> createService)
        {
            ReadService = readService;
            Mapper = mapper;
            CreateService = createService;
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

        public async Task<IActionResult> Create([FromBody] ImageDTO entityDTO)
        {
            try
            {
                var entity = Mapper.Map(entityDTO);
                int newId = await CreateService.Add(entity);
                return newId != default ? Ok(newId) : BadRequest("Entity already exists");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}
