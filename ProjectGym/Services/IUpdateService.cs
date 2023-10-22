namespace ProjectGym.Services
{
    public interface IUpdateService<T> where T : class
    {
        Task Update(T updatedEntity);
    }
}
