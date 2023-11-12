using Microsoft.AspNetCore.Mvc;
using ProjectGym.Services.Create;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    public interface ICreateController<TEntity, TDTO, TPK> where TEntity : class where TDTO : class
    {
        ICreateService<TEntity, TPK> CreateService { get; }

        Task<IActionResult> Create([FromBody] TDTO entityDTO);
    }
}
