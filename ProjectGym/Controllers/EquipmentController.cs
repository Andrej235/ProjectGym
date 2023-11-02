﻿using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    [Route("api/equipment")]
    [ApiController]
    public class EquipmentController : ControllerBase,
        IReadController<Equipment, EquipmentDTO, int>,
        ICreateController<Equipment, EquipmentDTO>,
        IUpdateController<Equipment>,
        IDeleteController<Equipment, int>
    {
        public IEntityMapperAsync<Equipment, EquipmentDTO> Mapper { get; }
        public IReadService<Equipment> ReadService { get; }
        public ICreateService<Equipment> CreateService { get; }
        public IUpdateService<Equipment> UpdateService { get; }
        public IDeleteService<Equipment> DeleteService { get; }

        public EquipmentController(IEntityMapperAsync<Equipment, EquipmentDTO> mapper,
                                   IReadService<Equipment> readService,
                                   ICreateService<Equipment> createService,
                                   IUpdateService<Equipment> updateService,
                                   IDeleteService<Equipment> deleteService)
        {
            ReadService = readService;
            Mapper = mapper;
            CreateService = createService;
            UpdateService = updateService;
            DeleteService = deleteService;
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

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EquipmentDTO updatedEntity)
        {
            try
            {
                var entity = await Mapper.Map(updatedEntity);
                await UpdateService.Update(entity);
                return Ok("Successfully updated entity");
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
                return Ok($"Entity with primary key {primaryKey} has been successfully deleted.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error occurred: {ex.Message}");
            }
        }
    }
}
