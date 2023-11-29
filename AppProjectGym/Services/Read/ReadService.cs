using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Services.Read
{
    public class ReadService<T> : IReadService<T> where T : class
    {
        public Task<T> Get(string primaryKey, string include = "all")
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> Get(string query, int? offset = 0, int? limit = -1, string include = "all")
        {
            throw new NotImplementedException();
        }
    }
}
