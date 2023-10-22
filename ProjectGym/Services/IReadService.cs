using System.Linq.Expressions;

namespace ProjectGym.Services
{
    public interface IReadService<T> where T : class
    {
        /// <summary>
        /// Finds a first entity in database which fits the provided criteria
        /// </summary>
        /// <param name="criteria">The method uses this criteria to search for a specific entity in the database</param>
        /// <param name="include">
        /// Include string should contain a list of propery names which will be included in the return
        /// <br/>Each item should be separated with a ; (semicolon)
        /// <br/>If one of the items is 'all' every property will be included
        /// <br/>If one of the items is 'none' no property will be included
        /// <br/>Cap insensitive
        /// </param>
        /// <returns>Return a first entity that fits the provided criteria, if such an entity doesn't exist a <see cref="NullReferenceException"/> will be thrown</returns>
        /// <exception cref="NullReferenceException"></exception>
        Task<T> Get(Expression<Func<T, bool>> criteria, string? include = "all");

        Task<List<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all");

        Task<List<T>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all");
    }
}
