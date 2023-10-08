using Microsoft.AspNetCore.Mvc;
using ProjectGym.Models;
using ProjectGym.Services;

namespace ProjectGym.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BasicGetDataService<ExerciseCategory> getDataService;
        public CategoryController(BasicGetDataService<ExerciseCategory> getDataService)
        {
            this.getDataService = getDataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await getDataService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await getDataService.Get());
    }
}
