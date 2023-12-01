using Microsoft.AspNetCore.Mvc;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public interface IUpdateController<TEntity, TDTO> where TEntity : class
    {
        IUpdateService<TEntity> UpdateService { get; }

        Task<IActionResult> Update([FromBody] TDTO updatedEntity);
    }
}
