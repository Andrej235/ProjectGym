﻿using Microsoft.AspNetCore.Mvc;
using ProjectGym.Models;
using ProjectGym.Services;

namespace ProjectGym.Controllers
{
    [Route("api/muscle")]
    [ApiController]
    public class MuscleController : ControllerBase
    {
        private readonly BasicGetDataService<Muscle> getDataService;
        public MuscleController(BasicGetDataService<Muscle> getDataService)
        {
            this.getDataService = getDataService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) => Ok(await getDataService.Get(id));

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await getDataService.Get());
    }
}
