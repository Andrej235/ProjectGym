namespace AppProjectGym.Services.Read
{
    public interface IReadService<T> where T : class
    {
        /// <summary>
        /// Retrieves a single entity by its primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key of the entity to retrieve.</param>
        /// <param name="include">Optional parameter specifying related entities to include. Default is "all".</param>
        /// <returns>The retrieved entity.</returns>
        Task<T> Get(string primaryKey, string include = "all");

        /// <summary>
        /// Retrieves a list of entities based on a query.
        /// </summary>
        /// <param name="query">
        /// The query string to filter entities. The format of the query string should follow these rules:
        /// <br/>- Key-value pairs are separated by a semi-colon.
        /// <br/>- Key names are not case sensitive.
        /// <br/>- Use the 'strict' key with a value of true to only return entities that fit all the given criteria.
        /// <br/>- A single key can contain multiple values separated by commas, and the result will contain entities that match ONE OF the given values for that key.
        /// <br/>Example query string: "name=MyName;myproperty=value1,value2,value3;strict=true"
        /// </param>
        /// <param name="offset">Optional parameter for specifying the offset. Default is 0.</param>
        /// <param name="limit">Optional parameter for specifying the limit. Default is -1 (no limit).</param>
        /// <param name="include">
        /// A comma-separated string specifying related entities to include. Accepted values are:
        /// <br/>- "none": Nothing will be included, regardless of other elements.
        /// <br/>- "all": Everything will be included.
        /// <br/>- List of entity names to include, separated by commas. Example: "entity1,entity2"
        /// <br/>(Note: The 'include' parameter is not case-sensitive.)
        /// </param>  
        /// <returns>The list of retrieved entities.</returns>
        Task<List<T>> Get(string query, int? offset = 0, int? limit = -1, string include = "all");
    }
}
