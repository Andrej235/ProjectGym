using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppProjectGym.Models;

namespace AppProjectGym.Services
{
    public class MuscleDataService : IDataService<Muscle>
    {
        public Task Create(Muscle newData)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Muscle>> Get()
        {
            throw new NotImplementedException();
        }

        public Task<Muscle> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task Get(OnDataLoad<Muscle> onDataLoad)
        {
            throw new NotImplementedException();
        }

        public Task Update(Muscle updatedData)
        {
            throw new NotImplementedException();
        }

        public Task Update(int id, Muscle updatedData)
        {
            throw new NotImplementedException();
        }
    }
}
