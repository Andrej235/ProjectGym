using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Mapping;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseService exerciseService;
        private readonly IReadService<Exercise> readService;
        private readonly IEntityMapper<Exercise, ExerciseDTO> mapper;

        public ExerciseController(ExerciseService exerciseService, IReadService<Exercise> readService, IEntityMapper<Exercise, ExerciseDTO> mapper)
        {
            this.exerciseService = exerciseService;
            this.readService = readService;
            this.mapper = mapper;
        }



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var exercises = await readService.Get(q, offset, limit, include);

            return Ok(
                exerciseService.TranslateToAdvancedDTO(
                    values: exercises.Select(mapper.MapEntity).ToList(),
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
                var exercise = await readService.Get(p => p.Id == id, include);
                return Ok((ExerciseDTO?)mapper.MapEntity(exercise));
            }
            catch (Exception)
            {
                return NotFound($"Exercise with id {id} was not found.");
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
