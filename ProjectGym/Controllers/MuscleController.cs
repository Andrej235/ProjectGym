using Microsoft.AspNetCore.Mvc;
using ProjectGym.Data;

namespace ProjectGym.Controllers
{
    [Route("api/muscle")]
    [ApiController]
    public class MuscleController : ControllerBase
    {
        private readonly ExerciseContext exerciseContext;

        public MuscleController(ExerciseContext exerciseContext)
        {
            this.exerciseContext = exerciseContext;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await exerciseContext.Muscles.FindAsync(id));
        }
    }
}
