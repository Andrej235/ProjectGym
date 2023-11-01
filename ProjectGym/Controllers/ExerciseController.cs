using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase, ICreateController<Exercise, ExerciseDTO>, IReadController<Exercise, ExerciseDTO>, IDeleteController<Exercise, int>
    {
        public IReadService<Exercise> ReadService { get; }
        public IEntityMapperAsync<Exercise, ExerciseDTO> Mapper { get; }
        public IDeleteService<Exercise> DeleteService { get; }
        public IDeleteService<ExerciseVariation> ExerciseVariationDeleteService { get; }
        public ICreateService<Exercise> CreateService { get; }

        public ExerciseController(IReadService<Exercise> readService, IEntityMapperAsync<Exercise, ExerciseDTO> mapper, IDeleteService<Exercise> deleteService, IDeleteService<ExerciseVariation> exerciseVariationDeleteService, ICreateService<Exercise> createService)
        {
            Mapper = mapper;
            CreateService = createService;
            ReadService = readService;
            DeleteService = deleteService;
            ExerciseVariationDeleteService = exerciseVariationDeleteService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            var exercises = await ReadService.Get(q, offset, limit, include);

            return Ok(
                AdvancedDTOMapper.TranslateToAdvancedDTO(
                    values: exercises.Select(Mapper.Map).ToList(),
                    baseAPIUrl: "exercise?" + (include != null ? $"&include={include}" : "") + (q != null ? $"&q={q}" : ""),
                    offset: offset ?? 0,
                    limit: limit ?? -1
                    )
                );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
        {
            try
            {
                var exercise = await ReadService.Get(p => p.Id == id, include);
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
        public async Task<IActionResult> Delete(int primaryKey)
        {
            try
            {
                await ExerciseVariationDeleteService.DeleteAll(x => x.Exercise1Id == primaryKey || x.Exercise2Id == primaryKey);
                await DeleteService.DeleteFirst(x => x.Id == primaryKey);
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
                var entity = await Mapper.Map(entityDTO);
                bool success = await CreateService.Add(entity);
                return success ? Ok("Successfully added entity to database") : BadRequest("Entity already exists");
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
