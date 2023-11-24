using Microsoft.AspNetCore.Mvc;
using ProjectGym.Services.Read;

namespace ProjectGym.Controllers
{
    public interface IReadController<TEntity, TDTO> where TEntity : class where TDTO : class
    {
        IReadService<TEntity> ReadService { get; }

        Task<IActionResult> Get(string id, [FromQuery] string? include);
        Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q);
    }
}
