using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using System.Linq.Expressions;

namespace ProjectGym.Services
{
    public class BasicGetDataService<T> where T : class
    {
        private readonly ExerciseContext context;
        public BasicGetDataService(ExerciseContext exerciseContext)
        {
            context = exerciseContext;
        }

        public async Task<T?> Get(int id) => await context.Set<T>().FindAsync(id);
        public async Task<List<T>> Get() => await context.Set<T>().ToListAsync();
        public async Task<List<T>> Get(Expression<Func<T, bool>> criteria) => await context.Set<T>().Where(criteria).ToListAsync();
    }
}
