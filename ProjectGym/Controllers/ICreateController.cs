using Microsoft.AspNetCore.Mvc;
using ProjectGym.Services.Create;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    public interface ICreateController<TEntity, TDTO> where TEntity : class where TDTO : class
    {
        ICreateService<TEntity> CreateService { get; }

        Task<IActionResult> Create([FromBody] TDTO entityDTO);
    }
}
