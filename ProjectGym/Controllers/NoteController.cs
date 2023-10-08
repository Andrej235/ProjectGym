using Microsoft.AspNetCore.Mvc;
using ProjectGym.Models;
using ProjectGym.Services;

namespace ProjectGym.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly BasicGetDataService<ExerciseNote> getDataService;
        public NoteController(BasicGetDataService<ExerciseNote> getDataService)
        {
            this.getDataService = getDataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await getDataService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await getDataService.Get());
    }
}
