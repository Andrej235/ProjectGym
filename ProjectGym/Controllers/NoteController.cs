using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [Route("api/note")]
    [ApiController]
    public class NoteController(ICreateService<Note> createService,
                                IReadService<Note> readService,
                                IEntityMapperSync<Note, NoteDTO> mapper,
                                IDeleteService<Note> deleteService) :
        ControllerBase,
        ICreateController<Note, NoteDTO>,
        IReadController<Note, NoteDTO>,
        IDeleteController<Note>
    {
        public ICreateService<Note> CreateService { get; } = createService;
        public IReadService<Note> ReadService { get; } = readService;
        public IDeleteService<Note> DeleteService { get; } = deleteService;
        public IEntityMapperSync<Note, NoteDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NoteDTO entityDTO)
        {
            try
            {
                var newEntityId = await CreateService.Add(Mapper.Map(entityDTO));
                if (newEntityId != default)
                    return Ok(newEntityId);
                else
                    return BadRequest("Entity already exists.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{primaryKey}")]
        public async Task<IActionResult> Delete(string primaryKey)
        {
            try
            {
                await DeleteService.Delete(primaryKey);
                return Ok("Successfully deleted entity.");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery] string? include)
        {
            try
            {
                return Ok(Mapper.Map(await ReadService.Get(id, include)));
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
    }
}
