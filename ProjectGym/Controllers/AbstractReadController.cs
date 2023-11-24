using ProjectGym.Services.Read;
using ProjectGym.Services.Mapping;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ProjectGym.Controllers
{
    [Obsolete("Doesn't work, kept as an idea")]
    public abstract class AbstractReadController<TEntity, TDTO, TPK>(IReadService<TEntity> readService, IEntityMapper<TEntity, TDTO> mapper) : ControllerBase where TEntity : class where TDTO : class
    {
        protected readonly IReadService<TEntity> readService = readService;
        protected readonly IEntityMapper<TEntity, TDTO> mapper = mapper;

        protected abstract Func<TEntity, TPK> PrimaryKey { get; }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(TPK id, [FromQuery] string? include)
        {
            try
            {
                var res = await readService.Get(x => ComparePrimaryKey(PrimaryKey(x), id), include);
                return Ok(mapper.Map(res));
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

        private bool ComparePrimaryKey(TPK x, TPK id)
        {
            if (x is int intPrimaryKey && id is int intId)
            {
                return intPrimaryKey == intId;
            }
            else if (x is Guid guidPrimaryKey && id is Guid guidId)
            {
                return guidPrimaryKey == guidId;
            }
            // Handle any other cases as needed, such as throwing an exception for unsupported types
            else
            {
                throw new ArgumentException("Unsupported primary key type");
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> Get([FromQuery] int? offset, [FromQuery] int? limit, [FromQuery] string? include, [FromQuery] string? q)
        {
            return Ok((await readService.Get(q, offset, limit, include)).Select(mapper.Map));
        }
    }
}
