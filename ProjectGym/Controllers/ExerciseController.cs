using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController(ICreateService<Exercise> createService,
                                    IReadService<Exercise> readService,
                                    IUpdateService<Exercise> updateService,
                                    IDeleteService<Exercise> deleteService,
                                    IEntityMapperAsync<Exercise, ExerciseDTO> mapper) : ControllerBase,
                                                                              ICreateController<Exercise, ExerciseDTO>,
                                                                              IReadController<Exercise, ExerciseDTO>,
                                                                              IUpdateController<Exercise, ExerciseDTO>,
                                                                              IDeleteController<Exercise>
    {
        public IReadService<Exercise> ReadService { get; } = readService;
        public IEntityMapperAsync<Exercise, ExerciseDTO> Mapper { get; } = mapper;
        public IDeleteService<Exercise> DeleteService { get; } = deleteService;
        public ICreateService<Exercise> CreateService { get; } = createService;
        public IUpdateService<Exercise> UpdateService { get; } = updateService;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var exercises = await ReadService.Get(q, offset, limit, include);

            return Ok(exercises.Select(Mapper.Map).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id, [FromQuery] string? include)
        {
            try
            {
                var exercise = await ReadService.Get(id, include);
                return Ok((ExerciseDTO?)Mapper.Map(exercise));
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
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ExerciseDTO entityDTO)
        {
            try
            {
                var entity = await Mapper.MapAsync(entityDTO);
                var newId = await CreateService.Add(entity);
                return newId != default ? Ok(newId) : BadRequest("Entity already exists");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }

        public async Task<IActionResult> Update([FromBody] ExerciseDTO updatedEntity)
        {
            try
            {
                await UpdateService.Update(await Mapper.MapAsync(updatedEntity));
                return Ok("Successfully updated entity");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
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
