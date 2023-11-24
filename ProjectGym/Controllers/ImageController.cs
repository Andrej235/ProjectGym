using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [Route("api/image")]
    [ApiController]
    public class ImageController(IReadService<Image> readService,
                                 IEntityMapperSync<Image, ImageDTO> mapper,
                                 ICreateService<Image, int> createService,
                                 IDeleteService<Image> deleteService)
        : ControllerBase,
        IReadController<Image, ImageDTO, int>,
        ICreateController<Image, ImageDTO, int>,
        IDeleteController<Image, int>
    {
        public IReadService<Image> ReadService { get; } = readService;
        public IEntityMapperSync<Image, ImageDTO> Mapper { get; } = mapper;
        public IDeleteService<Image> DeleteService { get; } = deleteService;
        public ICreateService<Image, int> CreateService { get; } = createService;

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
        {
            try
            {
                return Ok(Mapper.Map(await ReadService.Get(x => x.Id == id, include)));
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            try
            {
                var entities = (await ReadService.Get(q, offset, limit, include)).Select(Mapper.Map);
                return Ok(entities);
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpPost]
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

        [HttpDelete("{primaryKey}")]
        public async Task<IActionResult> Delete(int primaryKey)
        {
            try
            {
                await DeleteService.DeleteFirst(x => x.Id == primaryKey);
                return Ok("Successfully deleted entity.");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }
    }
}
