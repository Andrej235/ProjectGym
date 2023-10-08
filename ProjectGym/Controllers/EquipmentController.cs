using Microsoft.AspNetCore.Mvc;
using ProjectGym.Models;
using ProjectGym.Services;

namespace ProjectGym.Controllers
{
    [Route("api/equipment")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly BasicGetDataService<Equipment> getDataService;
        public EquipmentController(BasicGetDataService<Equipment> getDataService)
        {
            this.getDataService = getDataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await getDataService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await getDataService.Get());
    }
}
