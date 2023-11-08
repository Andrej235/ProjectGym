using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Services.Read
{
    public interface IReadService<T> where T : class
    {
        Task<T> Get(string primaryKey, string include = "all");
        Task<List<T>> Get(string query, int? offset = 0, int? limit = -1, string include = "all");
    }
}
