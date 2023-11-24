﻿using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Utilities;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/weight")]
    public class WeightController(ICreateService<PersonalExerciseWeight> createService,
                                  IEntityMapperSync<PersonalExerciseWeight, PersonalExerciseWeightDTO> mapper,
                                  IReadService<PersonalExerciseWeight> readService) : ControllerBase, ICreateController<PersonalExerciseWeight, PersonalExerciseWeightDTO>, IReadController<PersonalExerciseWeight, PersonalExerciseWeightDTO>
    {
        public ICreateService<PersonalExerciseWeight> CreateService { get; } = createService;
        public IReadService<PersonalExerciseWeight> ReadService { get; } = readService;
        public IEntityMapperSync<PersonalExerciseWeight, PersonalExerciseWeightDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonalExerciseWeightDTO entityDTO)
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
            catch (NullReferenceException)
            {
                return NotFound($"Entity with id {id} was not found.");
            }
            catch (Exception ex)
            {
                LogDebugger.LogError(ex);
                return BadRequest(LogDebugger.GetErrorMessage(ex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q) => Ok((await ReadService.Get(q, offset, limit, include)).Select(Mapper.Map));
    }
}
