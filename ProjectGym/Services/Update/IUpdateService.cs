namespace ProjectGym.Services.Update
{
    public interface IUpdateService<T> where T : class
    {
        /// <summary>
        /// Updates the provided entity in the database
        /// <br/>The provided entity MUST have the same primary key
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        Task Update(T updatedEntity);
    }
}
