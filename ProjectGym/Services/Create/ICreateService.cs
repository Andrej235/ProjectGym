namespace ProjectGym.Services.Create
{
    public interface ICreateService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Adds entity to database
        /// </summary>
        /// <returns>Id of added entity</returns>
        /// <param name="toAdd">Entity to save in the database</param>
        Task<object> Add(TEntity toAdd);
    }
}
