using AppProjectGym.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppProjectGym.Services
{
    public interface IExerciseSearchData
    {
        Task<List<Exercise>> Search(string q, string include = "none", int? offset = null, int? limit = null);
    }
}
