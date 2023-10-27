﻿using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Models;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    public interface IReadController<TEntity, TDTO> where TEntity : class where TDTO : class
    {
        IReadService<TEntity> ReadService { get; }
        IEntityMapper<TEntity, TDTO> Mapper { get; }

        public Task<IActionResult> Get(int id, [FromQuery] string? include);
        public Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q);
    }
}
