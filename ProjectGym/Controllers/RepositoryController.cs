using Microsoft.AspNetCore.Mvc;
using ProjectGym.Services.Create;
using ProjectGym.Services.Delete;
using ProjectGym.Services.Mapping;
using ProjectGym.Services.Read;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepositoryController<TEntity, TDTO, TMapper> 
        : ControllerBase,
        ICreateController<TEntity, TDTO>, 
        IReadController<TEntity, TDTO>, 
        IUpdateController<TEntity, TDTO>,
        IDeleteController<TEntity> 
        where TEntity : class
        where TDTO : class
        where TMapper : IEntityMapperSync<TEntity, TDTO>, IEntityMapperAsync<TEntity, TDTO>
    {
        public ICreateService<TEntity> CreateService => throw new NotImplementedException();
        public IUpdateService<TEntity> UpdateService => throw new NotImplementedException();
        public IDeleteService<TEntity> DeleteService => throw new NotImplementedException();
        public IReadService<TEntity> ReadService => throw new NotImplementedException();
        public TMapper Mapper => throw new NotImplementedException();

        public Task<IActionResult> Create([FromBody] TDTO entityDTO)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Delete(string primaryKey)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get(string id, [FromQuery] string? include)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Update([FromBody] TDTO updatedEntity)
        {
            throw new NotImplementedException();
        }
    }
}
