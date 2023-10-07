using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Services
{
    public delegate void OnDataLoad<T>(T input);
    public interface IDataService<T>
    {
        Task Create(T newData);
        Task<List<T>> Get();
        Task<T> Get(int id);
        Task Get(OnDataLoad<T> onDataLoad);
        Task Update(T updatedData);
        Task Update(int id, T updatedData);
        Task Delete(int id);
    }
}
