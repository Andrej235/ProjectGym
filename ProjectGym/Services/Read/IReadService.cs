using System.Linq.Expressions;

namespace ProjectGym.Services.Read
{
    public interface IReadService<T> where T : class
    {
        /// <summary>
        /// Finds a first entity in database which fits the provided criteria
        /// </summary>
        /// <param name="criteria">Criteria which is used to search for a specific entity in the database</param>
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

        /// <summary>
        /// Finds all entities in the database which fit the provided criteria
        /// </summary>
        /// <param name="criteria">Criteria which is used to search for specific entities in the database</param>
        /// <param name="offset">
        /// Number of entities which will be skipped when creating the output list
        /// <br/>If the value is 0, no entities will be skipped
        /// <br/>If the value is null, no entities will be skipped
        /// </param>
        /// <param name="limit">
        /// Maximum number of entities which will be included in the output list
        /// <br/>If a negative value is entered, output will include all entities which fit the criteria
        /// <br/>If the value is null, output will include all entities which fit the criteria
        /// </param>
        /// <param name="include">
        /// Include string should contain a list of propery names which will be included in the return
        /// <br/>Each item should be separated with a ; (semicolon)
        /// <br/>If one of the items is 'all' every property will be included
        /// <br/>If one of the items is 'none' no property will be included
        /// <br/>Cap insensitive
        /// </param>
        /// <returns>Return a list of entities that fit the provided criteria, if no such entity exists an empty list will be returned</returns>
        Task<List<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all");

        /// <summary>
        /// Finds all entities in the database which fit the provided query
        /// </summary>
        /// <param name="query">
        /// Query string should contain a list of key-value pairs separated with a ; (semicolon)
        /// Each item of the said list will be converted into a criteria and used in the final criteria
        /// The query can be strict or non-strict and it is determined by a key-value pair strict=true or strict=false
        /// If no strict key-value pair is provided the function uses strict=true
        /// If strict is enabled each entity in the return type will have to fit all of the provided criterias
        /// If strict is disabled each entity in the return type will have to fit at least one of the provided criterias and the output will be sorted to favor entities which fit the most criterias
        /// </param>
        /// <param name="offset">
        /// Number of entities which will be skipped when creating the output list
        /// <br/>If the value is 0, no entities will be skipped
        /// <br/>If the value is null, no entities will be skipped
        /// </param>
        /// <param name="limit">
        /// Maximum number of entities which will be included in the output list
        /// <br/>If a negative value is entered, output will include all entities which fit the criteria
        /// <br/>If the value is null, output will include all entities which fit the criteria
        /// </param>
        /// <param name="include">
        /// Include string should contain a list of propery names which will be included in the return
        /// <br/>Each item should be separated with a ; (semicolon)
        /// <br/>If one of the items is 'all' every property will be included
        /// <br/>If one of the items is 'none' no property will be included
        /// <br/>Cap insensitive
        /// </param>
        /// <returns>Return a list of entities that fit the provided query, if no such entity exists an empty list will be returned</returns>
        Task<List<T>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all");
    }
}
