using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Models;
using ProjectGym.Services;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseService exerciseService;

        public ExerciseController(ExerciseService exerciseService)
        {
            this.exerciseService = exerciseService;
        }



        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q) =>
            Ok(
                exerciseService.TranslateToAdvancedDTO(
                    values: exerciseService.TranslateToDTO(await exerciseService.Get(exerciseService.GetIncluded(include), q, offset, limit)),
                    url: include == null ? "/exercise" : $"/exercise?include={include}",
                    offset: offset ?? 0,
                    limit: limit ?? -1
                    )
                );

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include) => Ok(exerciseService.TranslateToDTO(await exerciseService.Get(p => p.Id == id, exerciseService.GetIncluded(include))));
    }
    public class AdvancedDTO<T>
    {
        public int BatchSize { get; set; }
        public string? PreviousBatchURLExtension { get; set; }
        public string? NextBatchURLExtension { get; set; }
        public List<T> Values { get; set; } = null!;
    }
}
