using Microsoft.AspNetCore.Mvc;
using ProjectGym.Models;
using ProjectGym.Services;

namespace ProjectGym.Controllers
{
    [Route("api/alias")]
    [ApiController]
    public class AliasController : ControllerBase
    {
        private readonly BasicGetDataService<ExerciseAlias> getDataService;
        public AliasController(BasicGetDataService<ExerciseAlias> getDataService)
        {
            this.getDataService = getDataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await getDataService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await getDataService.Get());
    }
}
