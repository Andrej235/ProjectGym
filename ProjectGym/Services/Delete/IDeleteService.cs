using System.Linq.Expressions;

namespace ProjectGym.Services.Delete
{
    public interface IDeleteService<T> where T : class
    {
        /// <summary>
        /// Deletes the first entity from the database which fits the provided criteria
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        Task DeleteFirst(Expression<Func<T, bool>> criteria);

        /// <summary>
        /// Deletes all entities from the database which fit the provided criteria
        /// </summary>
        /// <returns>
        /// true - if any entities were deleted
        /// <br/>false - if no entities were deleted
        /// </returns>
        Task<bool> DeleteAll(Expression<Func<T, bool>> criteria);
    }
}
