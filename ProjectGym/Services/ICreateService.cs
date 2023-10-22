namespace ProjectGym.Services
{
    public interface ICreateService<T> where T : class
    {
        /// <summary>
        /// Adds entity to database
        /// </summary>
        /// <param name="toAdd">Entity to save in the database</param>
        Task Add(T toAdd);
    }
}
