using System.Linq.Expressions;

namespace ProjectGym.Services
{
    public interface IDeleteService<T> where T : class
    {
        Task<bool> DeleteFirst(Expression<Func<T, bool>> expression);
        Task<bool> DeleteAll(Expression<Func<T, bool>> expression);
    }
}
