namespace ProjectGym.Services.Create
{
    public interface ICreateService<TEntity, TPK> where TEntity : class
    {
        /// <summary>
        /// Adds entity to database
        /// </summary>
        /// <returns>Id of added entity. default if the create action was unsuccessful</returns>
        /// <param name="toAdd">Entity to save in the database</param>
        Task<TPK> Add(TEntity toAdd);
    }
}
