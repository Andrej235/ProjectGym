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
    [Route("api/set")]
    public class SetController(IReadService<Set> readService,
                               ICreateService<Set, Guid> createService,
                               IEntityMapperSync<Set, SetDTO> mapper) : ControllerBase, ICreateController<Set, SetDTO, Guid>, IReadController<Set, SetDTO, Guid>
    {
        public IReadService<Set> ReadService { get; } = readService;
        public ICreateService<Set, Guid> CreateService { get; } = createService;
        public IEntityMapperSync<Set, SetDTO> Mapper { get; } = mapper;

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SetDTO entityDTO)
        {
            try
            {
                Guid newEntityId = await CreateService.Add(Mapper.Map(entityDTO));
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
        public async Task<IActionResult> Get(Guid id, [FromQuery] string? include)
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
    }
}
