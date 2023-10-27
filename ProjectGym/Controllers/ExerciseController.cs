using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase, IReadController<Exercise, ExerciseDTO>
    {
        public IReadService<Exercise> ReadService { get; }
        public IEntityMapper<Exercise, ExerciseDTO> Mapper { get; }
        public ExerciseController(IReadService<Exercise> readService, IEntityMapper<Exercise, ExerciseDTO> mapper)
        {
            ReadService = readService;
            Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var exercises = await ReadService.Get(q, offset, limit, include);

            return Ok(
                AdvancedDTOMapper.TranslateToAdvancedDTO(
                    values: exercises.Select(Mapper.Map).ToList(),
                    baseAPIUrl: "exercise?" + (include != null ? $"&include={include}" : "") + (q != null ? $"&q={q}" : ""),
                    offset: offset ?? 0,
                    limit: limit ?? -1
                    )
                );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
        {
            try
            {
                var exercise = await ReadService.Get(p => p.Id == id, include);
                return Ok((ExerciseDTO?)Mapper.Map(exercise));
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
    }
    public class AdvancedDTO<T>
    {
        public int BatchSize { get; set; }
        public string? PreviousBatchURLExtension { get; set; }
        public string? NextBatchURLExtension { get; set; }
        public List<T> Values { get; set; } = null!;
    }
}
