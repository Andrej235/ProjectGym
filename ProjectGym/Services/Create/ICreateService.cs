namespace ProjectGym.Services.Create
{
    public interface ICreateService<T> where T : class
    {
        /// <summary>
        /// Adds entity to database
        /// </summary>
        /// <param name="toAdd">Entity to save in the database</param>
        Task<bool> Add(T toAdd);
    }
}
