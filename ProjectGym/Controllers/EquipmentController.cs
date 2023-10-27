using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/equipment")]
    [ApiController]
    public class EquipmentController : ControllerBase, IReadController<Equipment, EquipmentDTO>, ICreateController<Equipment, EquipmentDTO>
    {
        public IReadService<Equipment> ReadService { get; }
        public IEntityMapperAsync<Equipment, EquipmentDTO> Mapper { get; }
        public ICreateService<Equipment> CreateService { get; }

        public EquipmentController(IReadService<Equipment> readService,
                                   ICreateService<Equipment> createService,
                                   IEntityMapperAsync<Equipment, EquipmentDTO> mapper)
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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EquipmentDTO entityDTO)
        {
            try
            {
                bool success = await CreateService.Add(await Mapper.Map(entityDTO));
                if (success)
                    return Ok("Successfully added entity to database.");
                else
                    return BadRequest("Entity already exists.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
