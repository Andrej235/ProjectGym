namespace ProjectGym.Services
{
    public interface ICRUDService<T> : ICreateService<T>, IReadService<T>, IUpdateService<T>, IDeleteService<T> where T : class
    {

    }
}
