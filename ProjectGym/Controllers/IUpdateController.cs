using Microsoft.AspNetCore.Mvc;
using ProjectGym.DTOs;
using ProjectGym.Services.Update;

namespace ProjectGym.Controllers
{
    public interface IUpdateController<TEntity> where TEntity : class
    {
        IUpdateService<TEntity> UpdateService { get; }

        Task<IActionResult> Update([FromBody] EquipmentDTO updatedEntity);
    }
}
