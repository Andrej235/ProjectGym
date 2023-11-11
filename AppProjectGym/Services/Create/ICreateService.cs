using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Services.Create
{
    public interface ICreateService<T> where T : class
    {
        Task<bool> Add(T entityToAdd);
    }
}
