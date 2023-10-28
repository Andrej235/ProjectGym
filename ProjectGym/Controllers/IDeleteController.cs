using Microsoft.AspNetCore.Mvc;
using ProjectGym.Services.Delete;
using System.Linq.Expressions;

namespace ProjectGym.Controllers
{
    public interface IDeleteController<TEntity, TPK> where TEntity : class
    {
        public IDeleteService<TEntity> DeleteService { get; }

        public Task<IActionResult> Delete(TPK primaryKey);
    }
}