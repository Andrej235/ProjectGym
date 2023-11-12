using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Services.Create
{
    public interface ICreateService<TEntity, TPK> where TEntity : class
    {
        Task<TPK> Add(TEntity entityToAdd);
    }
}
