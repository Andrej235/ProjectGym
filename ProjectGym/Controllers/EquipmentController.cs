﻿using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    [Route("api/equipment")]
    [ApiController]
    public class EquipmentController : ControllerBase, IReadController<Equipment, EquipmentDTO>
    {
        public IReadService<Equipment> ReadService { get; }
        public IEntityMapper<Equipment, EquipmentDTO> Mapper { get; }
        public EquipmentController(IReadService<Equipment> readService, IEntityMapper<Equipment, EquipmentDTO> mapper)
        {
            ReadService = readService;
            Mapper = mapper;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] string? include)
        {
            try
            {
                return Ok(Mapper.MapEntity(await ReadService.Get(p => p.Id == id, include)));
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
        public async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q) => Ok((await ReadService.Get(q, offset, limit, include)).Select(Mapper.MapEntity));
    }
}
